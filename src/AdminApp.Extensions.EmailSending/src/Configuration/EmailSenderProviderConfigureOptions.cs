// Use of this source code is governed by an MIT-style license that can be found in the LICENSE.txt file at the repository root.

using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Options;

namespace AdminApp.Extensions.EmailSending.Configuration
{
    internal class EmailSenderProviderConfigureOptions<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TOptions, TProvider> : ConfigureFromConfigurationOptions<TOptions> where TOptions : class
    {
        public EmailSenderProviderConfigureOptions(IEmailSenderProviderConfiguration<TProvider> providerConfiguration)
            : base(providerConfiguration.Configuration)
        {
        }
    }
}
