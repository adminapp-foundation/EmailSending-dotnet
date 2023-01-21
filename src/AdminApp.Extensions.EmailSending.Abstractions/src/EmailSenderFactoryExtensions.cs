// Use of this source code is governed by an MIT-style license that can be found in the LICENSE.txt file at the repository root.

using AdminApp.Extensions.Internal;

namespace AdminApp.Extensions.EmailSending
{
    public static class EmailSenderFactoryExtensions
    {
        public static IEmailSender<T> CreateEmailSender<T>(this IEmailSenderFactory factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            return new EmailSender<T>(factory);
        }

        public static IEmailSender CreateEmailSender(this IEmailSenderFactory factory, Type type)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return factory.CreateEmailSender(TypeNameHelper.GetTypeDisplayName(type, includeGenericParameters: false, nestedTypeDelimiter: '.'));
        }
    }
}
