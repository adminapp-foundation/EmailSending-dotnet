// Use of this source code is governed by an MIT-style license that can be found in the LICENSE.txt file at the repository root.

using AdminApp.Extensions.EmailSending;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class EmailSendingServiceCollectionExtensions
    {
        public static IServiceCollection AddEmailSending(this IServiceCollection services, Action<IEmailSenderBuilder> configure)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            // services.AddOptions();

            services.TryAdd(ServiceDescriptor.Singleton<IEmailSenderFactory, EmailSenderFactory>());
            services.TryAdd(ServiceDescriptor.Singleton(typeof(IEmailSender<>), typeof(EmailSender<>)));
            services.TryAddEnumerable(ServiceDescriptor.Singleton<IConfigureOptions<EmailSendingOptions>>(
                new ConfigureOptions<EmailSendingOptions>(options => { })));

            configure(new EmailSenderBuilder(services));

            return services;
        }
    }
}
