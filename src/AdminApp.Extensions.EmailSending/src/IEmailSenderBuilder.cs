// Use of this source code is governed by an MIT-style license that can be found in the LICENSE.txt file at the repository root.

using Microsoft.Extensions.DependencyInjection;

namespace AdminApp.Extensions.EmailSending
{
    public interface IEmailSenderBuilder
    {
        IServiceCollection Services { get; }
    }
}
