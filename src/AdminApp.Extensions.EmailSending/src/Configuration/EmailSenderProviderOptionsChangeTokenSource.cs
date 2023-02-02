// Use of this source code is governed by an MIT-style license that can be found in the LICENSE.txt file at the repository root.

using Microsoft.Extensions.Options;

namespace AdminApp.Extensions.EmailSending.Configuration
{
    public class EmailSenderProviderOptionsChangeTokenSource<TOptions, TProvider> : ConfigurationChangeTokenSource<TOptions>
    {        
        public EmailSenderProviderOptionsChangeTokenSource(IEmailSenderProviderConfiguration<TProvider> providerConfiguration) : base(providerConfiguration.Configuration)
        {
        }
    }
}
