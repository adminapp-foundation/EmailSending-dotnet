// Use of this source code is governed by an MIT-style license that can be found in the LICENSE.txt file at the repository root.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AdminApp.Extensions.EmailSending.Configuration
{
    public static class EmailSenderBuilderExtensions
    {
        public static IEmailSenderBuilder AddConfiguration(this IEmailSenderBuilder builder, IConfiguration configuration)
        {
            builder.AddConfiguration();

            builder.Services.AddSingleton<IConfigureOptions<EmailSendingOptions>>(new EmailSendingConfigureOptions(configuration));
            builder.Services.AddSingleton<IOptionsChangeTokenSource<EmailSendingOptions>>(new ConfigurationChangeTokenSource<EmailSendingOptions>(configuration));

            builder.Services.AddSingleton(new EmailSendingConfigureOptions(configuration));

            return builder;
        }
    }
}
