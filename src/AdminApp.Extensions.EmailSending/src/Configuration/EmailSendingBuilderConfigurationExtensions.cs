// Use of this source code is governed by an MIT-style license that can be found in the LICENSE.txt file at the repository root.

using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AdminApp.Extensions.EmailSending.Configuration
{
    public static class EmailSendingBuilderConfigurationExtensions
    {
        public static void AddConfiguration(this IEmailSenderBuilder builder)
        {
            builder.Services.TryAddSingleton<IEmailSenderProviderConfigurationFactory, EmailSenderProviderConfigurationFactory>();
            builder.Services.TryAddSingleton(typeof(IEmailSenderProviderConfiguration<>), typeof(EmailSenderProviderConfiguration<>));
        }
    }
}
