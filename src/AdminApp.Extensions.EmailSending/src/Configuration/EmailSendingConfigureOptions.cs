// Use of this source code is governed by an MIT-style license that can be found in the LICENSE.txt file at the repository root.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace AdminApp.Extensions.EmailSending.Configuration
{
    internal class EmailSendingConfigureOptions : IConfigureOptions<EmailSendingOptions>
    {
        private readonly IConfiguration _configuration;

        public EmailSendingConfigureOptions(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public void Configure(EmailSendingOptions options)
        {
            LoadDefaultConfigValues(options);
        }

        private void LoadDefaultConfigValues(EmailSendingOptions options)
        {
            if (_configuration == null)
            {
                return;
            }

            var preferredProviders = _configuration.GetValue<List<string>>(nameof(options.ProviderOrders));
            if (preferredProviders != null && preferredProviders.Any())
            {
                options.ProviderOrders.Clear();
                foreach (var provider in preferredProviders)
                {
                    options.ProviderOrders.Add(provider);
                }
            }

            var fromRules = _configuration.GetValue<Dictionary<string, List<string>>>(nameof(options.FromRules));
            if (fromRules != null && fromRules.Any())
            {
                foreach (var fromRule in fromRules)
                {
                    options.FromRules.TryAdd(fromRule.Key, fromRule.Value);
                }
            }

            var categoryRules = _configuration.GetValue<Dictionary<string, CategoryRuleConfigureValue>>(nameof(options.CategoryRules));
            if (categoryRules != null && categoryRules.Any())
            {
                foreach (var categoryRule in categoryRules)
                {
                    var categoryRuleValue = categoryRule.Value;
                    if (categoryRuleValue != null && categoryRuleValue.FromEmailAddress != null)
                    {
                        var added = options.CategoryRules.TryAdd(
                            categoryRule.Key,
                            new CategoryRuleValue(categoryRuleValue.FromEmailAddress, categoryRuleValue.ReplyToEmailAddress));
                        if (!added)
                        {
                            var existingValue = options.CategoryRules[categoryRule.Key];
                            if (existingValue != null)
                            {
                                if(string.IsNullOrWhiteSpace(existingValue.FromEmailAddress.Name) &&
                                    !string.IsNullOrWhiteSpace(categoryRuleValue.FromEmailAddress.Name))
                                {
                                    existingValue.FromEmailAddress.Name = categoryRuleValue.FromEmailAddress.Name;
                                }

                                if(existingValue.ReplyToEmailAddress == null)
                                {
                                    existingValue.ReplyToEmailAddress = categoryRuleValue.ReplyToEmailAddress;
                                } else if(string.IsNullOrWhiteSpace(existingValue.ReplyToEmailAddress.Name) &&
                                    categoryRuleValue.ReplyToEmailAddress != null &&
                                    !string.IsNullOrWhiteSpace(categoryRuleValue.ReplyToEmailAddress.Name))
                                {
                                    existingValue.ReplyToEmailAddress.Name = categoryRuleValue.ReplyToEmailAddress.Name;
                                }
                            }                       
                        }
                    }
                }
            }
        }

        public class CategoryRuleConfigureValue
        {
            public string? From { get; set; }
            public string? ReplyTo { get; set; }

            internal EmailAddress? FromEmailAddress
            {
                get
                {
                    return ParseEmailAddress(From);
                }
            }

            internal EmailAddress? ReplyToEmailAddress
            {
                get
                {
                    return ParseEmailAddress(ReplyTo);
                }
            }

            private static EmailAddress? ParseEmailAddress(string? emailText)
            {
                if (string.IsNullOrWhiteSpace(emailText))
                {
                    return null;
                }

                try
                {
                    var mailAddress = new System.Net.Mail.MailAddress(emailText);

                    return new EmailAddress(mailAddress.Address, mailAddress.DisplayName);
                }
                catch (FormatException)
                {
                    return null;
                }
            }
        }
    }
}
