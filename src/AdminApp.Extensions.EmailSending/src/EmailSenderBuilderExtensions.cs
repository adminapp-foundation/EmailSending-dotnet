// Use of this source code is governed by an MIT-style license that can be found in the LICENSE.txt file at the repository root.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AdminApp.Extensions.EmailSending
{
    public static class EmailSenderBuilderExtensions
    {
        public static IEmailSenderBuilder AddProvider(this IEmailSenderBuilder builder, IEmailSenderProvider provider)
        {
            builder.Services.AddSingleton(provider);

            return builder;
        }

        public static IEmailSenderBuilder ClearProviders(this IEmailSenderBuilder builder)
        {
            builder.Services.RemoveAll<IEmailSenderProvider>();

            return builder;
        }

        public static IEmailSenderBuilder Configure(this IEmailSenderBuilder builder, Action<EmailSendingOptions> action)
        {
            builder.Services.Configure(action);

            return builder;
        }
    }
}
