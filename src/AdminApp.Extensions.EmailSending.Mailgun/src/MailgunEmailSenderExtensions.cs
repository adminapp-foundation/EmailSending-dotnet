// Use of this source code is governed by an MIT-style license that can be found in the LICENSE.txt file at the repository root.

using AdminApp.Extensions.EmailSending.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AdminApp.Extensions.EmailSending.Mailgun
{
    public static class MailgunEmailSenderExtensions
    {
        public static IEmailSenderBuilder AddMailgun(this IEmailSenderBuilder builder)
        {
            builder.AddConfiguration();            
            

            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IEmailSenderProvider, MailgunEmailSenderProvider>());
            EmailSenderProviderOptions.RegisterProviderOptions<MailgunOptions, MailgunEmailSenderProvider>(builder.Services);

            return builder;
        }

        public static IEmailSenderBuilder AddMailgun(this IEmailSenderBuilder builder, Action<MailgunOptions> configure)
        {
            builder.AddMailgun();
            builder.Services.Configure(configure);

            return builder;
        }
    }
}
