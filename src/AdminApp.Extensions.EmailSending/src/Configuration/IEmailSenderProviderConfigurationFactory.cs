// Use of this source code is governed by an MIT-style license that can be found in the LICENSE.txt file at the repository root.

using Microsoft.Extensions.Configuration;

namespace AdminApp.Extensions.EmailSending.Configuration
{
    internal interface IEmailSenderProviderConfigurationFactory
    {
        IConfiguration GetConfiguration(Type providerType);
    }
}
