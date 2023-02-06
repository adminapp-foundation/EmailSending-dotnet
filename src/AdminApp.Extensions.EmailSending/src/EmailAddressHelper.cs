// Use of this source code is governed by an MIT-style license that can be found in the LICENSE.txt file at the repository root.

namespace AdminApp.Extensions.EmailSending
{
    public static class EmailAddressHelper
    {
        public static EmailAddress? ParseEmail(string emailText, bool? throwIfFormatException = false)
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
            catch (FormatException ex)
            {
                if (throwIfFormatException == true)
                {
                    throw ex;
                }

                return null;
            }
        }
    }
}
