// Use of this source code is governed by an MIT-style license that can be found in the LICENSE.txt file at the repository root.

using System.Collections.Concurrent;
using Microsoft.Extensions.Options;

namespace AdminApp.Extensions.EmailSending.Mailgun
{
    [EmailSenderProviderAlias("Mailgun")]
    public class MailgunEmailSenderProvider : IEmailSenderProvider
    {
        private readonly IOptionsMonitor<MailgunOptions> _optionsDelegate;
        private readonly ConcurrentDictionary<string, MailgunEmailSender> _emailSenders;

        private IDisposable? _optionsReloadToken;

        public MailgunEmailSenderProvider(IOptionsMonitor<MailgunOptions> optionsDelegate)
        {
            _optionsDelegate = optionsDelegate;
            _emailSenders = new ConcurrentDictionary<string, MailgunEmailSender>();

            ReloadOptions(_optionsDelegate.CurrentValue);
            _optionsReloadToken = _optionsDelegate.OnChange(ReloadOptions);
        }

        public IEmailSender CreateEmailSender(string categoryName)
        {
            return _emailSenders.TryGetValue(categoryName, out MailgunEmailSender? emailSender) ?
            emailSender :
            _emailSenders.GetOrAdd(categoryName, new MailgunEmailSender(categoryName, _optionsDelegate.CurrentValue));
        }

        private void ReloadOptions(MailgunOptions options)
        {
            foreach (KeyValuePair<string, MailgunEmailSender> emailSender in _emailSenders)
            {
                emailSender.Value.Options = options;
            }
        }


        public void Dispose()
        {
            _optionsReloadToken?.Dispose();
        }
    }
}
