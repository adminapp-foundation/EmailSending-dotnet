// Use of this source code is governed by an MIT-style license that can be found in the LICENSE.txt file at the repository root.

using Microsoft.Extensions.Configuration;

namespace AdminApp.Extensions.EmailSending.Configuration
{
    internal sealed class EmailSenderProviderConfiguration<T> : IEmailSenderProviderConfiguration<T>
    {
        public EmailSenderProviderConfiguration(IEmailSenderProviderConfigurationFactory providerConfigurationFactory)
        {
            Configuration = providerConfigurationFactory.GetConfiguration(typeof(T));
        }

        public IConfiguration Configuration { get; }
    }
}
