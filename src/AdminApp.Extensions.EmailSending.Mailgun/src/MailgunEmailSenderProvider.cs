// Use of this source code is governed by an MIT-style license that can be found in the LICENSE.txt file at the repository root.

namespace AdminApp.Extensions.EmailSending.Mailgun
{
    public class MailgunEmailSenderProvider : IEmailSenderProvider
    {
        public IEmailSender CreateEmailSender(string categoryName) => throw new NotImplementedException();
        public void Dispose() => throw new NotImplementedException();
    }
}
