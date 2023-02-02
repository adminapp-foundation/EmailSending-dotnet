// Use of this source code is governed by an MIT-style license that can be found in the LICENSE.txt file at the repository root.

// Done

using Microsoft.Extensions.Configuration;

namespace AdminApp.Extensions.EmailSending.Configuration
{
    internal sealed class EmailSenderProviderConfigurationFactory : IEmailSenderProviderConfigurationFactory
    {
        private readonly IEnumerable<EmailSendingConfiguration> _configurations;

        public EmailSenderProviderConfigurationFactory(IEnumerable<EmailSendingConfiguration> configurations)
        {
            _configurations = configurations;
        }

        public IConfiguration GetConfiguration(Type providerType)
        {
            string fullName = providerType.FullName!;
            string? alias = ProviderAliasUtilities.GetAlias(providerType);
            var configurationBuilder = new ConfigurationBuilder();
            foreach (EmailSendingConfiguration configuration in _configurations)
            {
                IConfigurationSection sectionFromFullName = configuration.Configuration.GetSection(fullName);
                configurationBuilder.AddConfiguration(sectionFromFullName);

                if (!string.IsNullOrWhiteSpace(alias))
                {
                    IConfigurationSection sectionFromAlias = configuration.Configuration.GetSection(alias);
                    configurationBuilder.AddConfiguration(sectionFromAlias);
                }
            }

            return configurationBuilder.Build();
        }
    }
}
