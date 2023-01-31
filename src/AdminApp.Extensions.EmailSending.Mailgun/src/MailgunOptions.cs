// Use of this source code is governed by an MIT-style license that can be found in the LICENSE.txt file at the repository root.

namespace AdminApp.Extensions.EmailSending.Mailgun
{
    public class MailgunOptions
    {
        public string? ApiKey { get; set; }

        public EmailAddress? FromEmailAddress { get; set; }

        public IList<MailgunFromEmailRule>? FromEmailRules { get; set; } = new List<MailgunFromEmailRule>();
    }

    public class MailgunFromEmailRule
    {
        public MailgunFromEmailRule(EmailAddress fromEmailAddress)
        {
            FromEmailAddress = fromEmailAddress;

        }
        public string? ApiKey { get; set; }
        public EmailAddress FromEmailAddress { get; set; }
    }
}
