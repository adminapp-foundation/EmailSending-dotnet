// Use of this source code is governed by an MIT-style license that can be found in the LICENSE.txt file at the repository root.

namespace AdminApp.Extensions.EmailSending
{
    public class EmailMessage
    {
        public EmailMessage(IList<EmailAddress> toEmailAddresses, string subject, string plainText, string htmlText)
        {
            ToEmailAddresses = toEmailAddresses;
            Subject = subject;
            PlainText = plainText;
            HtmlText = htmlText;
        }

        public IList<EmailAddress> ToEmailAddresses { get; set; }
        public string Subject { get; set; }
        public string PlainText { get; set; }
        public string HtmlText { get; set; }
        public EmailAddress? FromEmailAddress { get; set; }
        public EmailAddress? ReplyToEmailAddress { get; set; }
        public IList<EmailAddress>? CcEmailAddresses { get; set; } = null;
        public IList<EmailAddress>? BccEmailAddresses { get; set; } = null;
        public IList<Attachment>? Attachments { get; set; } = null;
        public IList<Attachment>? InlineImages { get; set; } = null;
        public bool? ClickTrackingEnabled { get; set; } = null;
        public long? SendAt { get; set; }
        public IList<string>? Tags { get; set; } = null;
        public IDictionary<string, Dictionary<string, string>>? RecipientSubstitutions { get; set; } = null;
        public IDictionary<string, string>? GlobalSubstitutions { get; set; } = null;
    }
}
