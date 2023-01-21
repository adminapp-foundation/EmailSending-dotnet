// Use of this source code is governed by an MIT-style license that can be found in the LICENSE.txt file at the repository root.

using AdminApp.Extensions.Internal;

namespace AdminApp.Extensions.EmailSending
{
    public class EmailSender<T> : IEmailSender<T>
    {
        private readonly IEmailSender _emailSender;

        public EmailSender(IEmailSenderFactory factory)
        {
            if(factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            _emailSender = factory.CreateEmailSender(TypeNameHelper.GetTypeDisplayName(typeof(T), includeGenericParameters: false, nestedTypeDelimiter: '.'));
        }


        public bool IsEnabled(string? fromEmail)
        {
            return _emailSender.IsEnabled(fromEmail);
        }

        public Task<EmailSendingResult> SendAsync(EmailMessage emailMessage, CancellationToken cancellationToken = default)
        {
            return _emailSender.SendAsync(emailMessage, cancellationToken);
        }
    }
}
