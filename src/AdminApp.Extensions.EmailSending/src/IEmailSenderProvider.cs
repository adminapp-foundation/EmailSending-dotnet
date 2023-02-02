// Use of this source code is governed by an MIT-style license that can be found in the LICENSE.txt file at the repository root.

namespace AdminApp.Extensions.EmailSending
{
    public interface IEmailSenderProvider : IDisposable
    {
        IEmailSender CreateEmailSender(string categoryName);
    }
}
