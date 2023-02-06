// Use of this source code is governed by an MIT-style license that can be found in the LICENSE.txt file at the repository root.

namespace AdminApp.Extensions.EmailSending.Mailgun
{
    public class MailgunOptions
    {
        public string? ApiKey { get; set; }

        public string? From { get; set; }

        internal EmailAddress? FromEmailAddress
        {
            get
            {
                return From != null ? EmailAddressHelper.ParseEmail(From) : null;
            }
        }

        public IList<MailgunRule>? Rules { get; set; }
    }

    public class MailgunRule
    {
        public string? From { get; set; }

        public string? ApiKey { get; set; }

        internal EmailAddress? FromEmailAddress
        {
            get
            {
                return From != null ? EmailAddressHelper.ParseEmail(From) : null;
            }
        }
    }
}
