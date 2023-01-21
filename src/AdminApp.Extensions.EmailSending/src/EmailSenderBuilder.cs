// Use of this source code is governed by an MIT-style license that can be found in the LICENSE.txt file at the repository root.

using Microsoft.Extensions.DependencyInjection;

namespace AdminApp.Extensions.EmailSending
{
    public class EmailSenderBuilder : IEmailSenderBuilder
    {
        public EmailSenderBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; }
    }
}
