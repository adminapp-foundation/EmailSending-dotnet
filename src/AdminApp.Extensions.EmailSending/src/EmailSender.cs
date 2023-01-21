// Use of this source code is governed by an MIT-style license that can be found in the LICENSE.txt file at the repository root.

using System;
using AdminApp.Extensions.EmailSending.Resources;

namespace AdminApp.Extensions.EmailSending
{
    internal sealed class EmailSender : IEmailSender
    {
        public EmailSenderProviderInfo[] EmailSenderProviderInfoes { get; set; }
        public EmailSenderInfo[]? PreferredEmailSenders { get; set; }

        public EmailSender(EmailSenderProviderInfo[] emailSenderProviderInfoes)
        {
            EmailSenderProviderInfoes = emailSenderProviderInfoes;
        }

        public async Task<EmailSendingResult> SendAsync(EmailMessage emailMessage, CancellationToken cancellationToken = default)
        {
            if (PreferredEmailSenders == null)
            {
                return new EmailSendingResult(false, null, SR.NoEmailProvider);
            }

            EmailSenderInfo[] emailSenders = PreferredEmailSenders;

            List<string> errorMessages = new List<string>();

            foreach (EmailSenderInfo emailSenderInfo in emailSenders)
            {                
                emailMessage.FromEmailAddress ??= emailSenderInfo.FromEmailAddress;
                emailMessage.ReplyToEmailAddress ??= emailSenderInfo.ReplyToEmailAddress;
                string? fromEmail = emailMessage.FromEmailAddress?.Email;

                if (!emailSenderInfo.EmailSender.IsEnabled(fromEmail))
                {
                    continue;
                }             

                EmailSendingResult result = await emailSenderInfo.EmailSender.SendAsync(emailMessage, cancellationToken);
                if (result.IsSuccess)
                {
                    return result;
                }

                if (result.Message != null)
                {
                    errorMessages.Add(result.Message);
                }
            }

            return new EmailSendingResult(false, null, $"{SR.EmailSendingError}.\n{string.Join("\n", errorMessages)}");
        }

        public bool IsEnabled(string? fromEmail)
        {
            if (PreferredEmailSenders == null)
            {
                return false;
            }

            EmailSenderInfo[] emailSenders = PreferredEmailSenders;

            List<Exception>? exceptions = null;
            int i = 0;
            for (; i < emailSenders.Length; i++)
            {
                ref readonly EmailSenderInfo emailSenderInfo = ref emailSenders[i];

                if (EmailSenderIsEnabled(fromEmail, emailSenderInfo.EmailSender, ref exceptions))
                {
                    break;
                }
            }

            if (exceptions != null && exceptions.Count > 0)
            {
                throw new AggregateException(message: SR.EmailSendingError, innerExceptions: exceptions);
            }

            return i < emailSenders.Length;

            static bool EmailSenderIsEnabled(string? fromEmail, IEmailSender emailSender, ref List<Exception>? exceptions)
            {
                try
                {
                    if (emailSender.IsEnabled(fromEmail))
                    {
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    exceptions ??= new List<Exception>();
                    exceptions.Add(ex);
                }

                return false;
            }
        }
    }
}
