// Use of this source code is governed by an MIT-style license that can be found in the LICENSE.txt file at the repository root.

using System.Data;
using System.Diagnostics.CodeAnalysis;
using AdminApp.Extensions.EmailSending.Resources;
using Microsoft.Extensions.Options;

namespace AdminApp.Extensions.EmailSending
{
    public class EmailSenderFactory : IEmailSenderFactory
    {
        #region Fields

        private readonly object _sync = new();
        private readonly Dictionary<string, EmailSender> _emailSenders = new(StringComparer.Ordinal);
        private readonly List<ProviderRegistration> _providerRegistrations = new();
        private EmailSenderOptions _options;
        private IDisposable? _changeTokenRegistration;

        private volatile bool _disposed;

        #endregion

        #region Constructors

        public EmailSenderFactory() : this(Array.Empty<IEmailSenderProvider>())
        {
        }

        public EmailSenderFactory(IEnumerable<IEmailSenderProvider> providers) : this(providers, new StaticOptionsMonitor(new EmailSenderOptions()))
        {
        }

        public EmailSenderFactory(IEnumerable<IEmailSenderProvider> providers, EmailSenderOptions options)
            : this(providers, new StaticOptionsMonitor(options))
        {
        }

        public EmailSenderFactory(IEnumerable<IEmailSenderProvider> providers, IOptionsMonitor<EmailSenderOptions> optionsDelegate)
        {
            foreach (IEmailSenderProvider provider in providers)
            {
                AddProviderRegistration(provider, shouldDispose: false);
            }

            _changeTokenRegistration = optionsDelegate.OnChange(RefreshFilters);
            RefreshFilters(optionsDelegate.CurrentValue);
        }

        #endregion

        #region Public Methods

        public IEmailSender CreateEmailSender(string categoryName)
        {
            if (CheckDisposed())
            {
                throw new ObjectDisposedException(nameof(EmailSenderFactory));
            }

            lock (_sync)
            {
                if (!_emailSenders.TryGetValue(categoryName, out EmailSender? emailSender))
                {
                    emailSender = new EmailSender(CreateEmailSenderProviderInfoes(categoryName));
                    emailSender.PreferredEmailSenders = ApplyFilters(emailSender.EmailSenderProviderInfoes);
                    _emailSenders[categoryName] = emailSender;
                }

                return emailSender;
            }
        }

        public void AddProvider(IEmailSenderProvider provider)
        {
            if (CheckDisposed())
            {
                throw new ObjectDisposedException(nameof(EmailSenderFactory));
            }

            if (provider is null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            lock (_sync)
            {
                AddProviderRegistration(provider, shouldDispose: true);

                foreach (KeyValuePair<string, EmailSender> existingEmailSender in _emailSenders)
                {
                    EmailSender emailSender = existingEmailSender.Value;
                    EmailSenderProviderInfo[] emailSenderProviderInfoes = emailSender.EmailSenderProviderInfoes;

                    int newEmailSenderProviderInfoesLength = emailSenderProviderInfoes.Length;
                    Array.Resize(ref emailSenderProviderInfoes, emailSenderProviderInfoes.Length + 1);
                    emailSenderProviderInfoes[newEmailSenderProviderInfoesLength] = new EmailSenderProviderInfo(provider, existingEmailSender.Key);

                    emailSender.EmailSenderProviderInfoes = emailSenderProviderInfoes;
                    emailSender.PreferredEmailSenders = ApplyFilters(emailSender.EmailSenderProviderInfoes);
                }
            }
        }

        #endregion

        #region Private Methods

        private void AddProviderRegistration(IEmailSenderProvider provider, bool shouldDispose)
        {
            _providerRegistrations.Add(new ProviderRegistration
            {
                Provider = provider,
                ShouldDispose = shouldDispose
            });
        }

        private EmailSenderProviderInfo[] CreateEmailSenderProviderInfoes(string categoryName)
        {
            var emailSenderProviderInfoes = new EmailSenderProviderInfo[_providerRegistrations.Count];
            for (int i = 0; i < _providerRegistrations.Count; i++)
            {
                emailSenderProviderInfoes[i] = new EmailSenderProviderInfo(_providerRegistrations[i].Provider, categoryName);
            }

            return emailSenderProviderInfoes;
        }

        [MemberNotNull(nameof(_options))]
        private void RefreshFilters(EmailSenderOptions options)
        {
            lock (_sync)
            {
                _options = options;
                foreach (KeyValuePair<string, EmailSender> registeredEmailSender in _emailSenders)
                {
                    var emailSender = registeredEmailSender.Value;
                    emailSender.PreferredEmailSenders = ApplyFilters(emailSender.EmailSenderProviderInfoes);
                }
            }
        }

        private EmailSenderInfo[] ApplyFilters(EmailSenderProviderInfo[] emailSenderProviderInfoes)
        {
            var filteredEmailSenderInfoes = new List<EmailSenderInfo>();
            var preferredProviderInfoes = GetPreferredProviderInfoes(emailSenderProviderInfoes);

            foreach (var preferredProviderInfo in preferredProviderInfoes)
            {
                foreach (EmailSenderProviderInfo emailSenderProviderInfo in emailSenderProviderInfoes)
                {
                    if (preferredProviderInfo.ProviderName == emailSenderProviderInfo.ProviderTypeFullName
                        || preferredProviderInfo.ProviderName == emailSenderProviderInfo.ProviderAlias)
                    {
                        filteredEmailSenderInfoes.Add(new EmailSenderInfo(
                            emailSenderProviderInfo.EmailSender,
                            emailSenderProviderInfo.Category,
                            preferredProviderInfo.FromEmailAddress, preferredProviderInfo.ReplyToEmailAddress));
                        break;
                    }
                }
            }

            if (!filteredEmailSenderInfoes.Any())
            {
                foreach (EmailSenderProviderInfo emailSenderProviderInfo in emailSenderProviderInfoes)
                {
                    filteredEmailSenderInfoes.Add(new EmailSenderInfo(
                        emailSenderProviderInfo.EmailSender,
                        emailSenderProviderInfo.Category,
                        null,
                        null));
                }
            }

            return filteredEmailSenderInfoes.ToArray();
        }

        private IList<PreferredProviderInfo> GetPreferredProviderInfoes(EmailSenderProviderInfo[] emailSenderProviderInfoes)
        {
            foreach (var emailSenderProviderInfo in emailSenderProviderInfoes)
            {
                CategoryEmailRule? currentCategoryEmailRule = null;
                foreach (var rule in _options.InternalCategoryEmailRules)
                {
                    if (IsBetterCategoryEmailRule(rule, currentCategoryEmailRule, emailSenderProviderInfo.Category))
                    {
                        currentCategoryEmailRule = rule;
                    }
                }

                if (currentCategoryEmailRule != null)
                {
                    var fromEmail = currentCategoryEmailRule.FromEmailAddress.Email;

                    foreach (FromEmailProviderNameRule rule in _options.InternalFromEmailProviderNameRules)
                    {
                        if (fromEmail.ToUpperInvariant() == rule.FromEmail.ToUpperInvariant())
                        {
                            return rule.ProviderNames
                                .ToList()
                                .Select(providerName =>
                                new PreferredProviderInfo(
                                    providerName,
                                    currentCategoryEmailRule.FromEmailAddress,
                                    currentCategoryEmailRule.ReplyToEmailAddress)
                                )
                                .ToList();

                        }
                    }
                }
            }

            if (_options.InternalPreferredProviderNames.Any())
            {
                return _options.InternalPreferredProviderNames
                              .ToList()
                              .Select(providerName => new PreferredProviderInfo(providerName, null, null))
                              .ToList();
            }

            return new List<PreferredProviderInfo>();
        }

        private static bool IsBetterCategoryEmailRule(CategoryEmailRule rule, CategoryEmailRule? current, string category)
        {
            string? categoryName = rule.CategoryName;
            if (categoryName != null)
            {
                const char WildcardChar = '*';

                int wildcardIndex = categoryName.IndexOf(WildcardChar);
                if (wildcardIndex != -1 &&
                    categoryName.IndexOf(WildcardChar, wildcardIndex + 1) != -1)
                {
                    throw new InvalidOperationException(SR.MoreThanOneWildcard);
                }

                ReadOnlySpan<char> prefix, suffix;
                if (wildcardIndex == -1)
                {
                    prefix = categoryName.AsSpan();
                    suffix = default;
                }
                else
                {
                    prefix = categoryName.AsSpan(0, wildcardIndex);
                    suffix = categoryName.AsSpan(wildcardIndex + 1);
                }

                if (!category.AsSpan().StartsWith(prefix, StringComparison.OrdinalIgnoreCase) ||
                    !category.AsSpan().EndsWith(suffix, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }

            if (current?.CategoryName != null)
            {
                if (rule.CategoryName == null)
                {
                    return false;
                }

                if (current.CategoryName.Length > rule.CategoryName.Length)
                {
                    return false;
                }
            }

            return true;
        }

        #endregion

        #region Dispose

        protected virtual bool CheckDisposed() => _disposed;

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;

                _changeTokenRegistration?.Dispose();

                foreach (ProviderRegistration registration in _providerRegistrations)
                {
                    try
                    {
                        if (registration.ShouldDispose)
                        {
                            registration.Provider.Dispose();
                        }
                    }
                    catch
                    {
                        // Swallow exceptions on dispose
                    }
                }
            }
        }

        #endregion

        #region Private Classes/Structs

        private struct ProviderRegistration
        {
            public IEmailSenderProvider Provider;
            public bool ShouldDispose;
        }

        private readonly struct PreferredProviderInfo
        {
            public PreferredProviderInfo(string providerName, EmailAddress? fromEmailAddress, EmailAddress? replyToEmailAddress)
            {
                ProviderName = providerName;
                FromEmailAddress = fromEmailAddress;
                ReplyToEmailAddress = replyToEmailAddress;

            }
            public string ProviderName { get; }
            public EmailAddress? FromEmailAddress { get; }
            public EmailAddress? ReplyToEmailAddress { get; }
        }

        #endregion
    }
}
