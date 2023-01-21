// Use of this source code is governed by an MIT-style license that can be found in the LICENSE.txt file at the repository root.

namespace AdminApp.Extensions.EmailSending
{
    internal readonly struct EmailSenderInfo
    {
        public EmailSenderInfo(IEmailSender emailSender, string? category, EmailAddress? fromEmailAddress, EmailAddress? replyToEmailAddress)
        {
            EmailSender = emailSender;
            Category = category;
            FromEmailAddress = fromEmailAddress;
            ReplyToEmailAddress = replyToEmailAddress;
        }

        public IEmailSender EmailSender { get; }

        public string? Category { get; }

        public EmailAddress? FromEmailAddress { get; }

        public EmailAddress? ReplyToEmailAddress { get; }        
    }

    internal readonly struct EmailSenderProviderInfo
    {
        public EmailSenderProviderInfo(IEmailSenderProvider provider, string category) : this()
        {
            ProviderType = provider.GetType();
            ProviderTypeFullName = ProviderType.FullName;
            ProviderTypeFullName = ProviderAliasUtilities.GetAlias(ProviderType);

            EmailSender = provider.CreateEmailSender(category);
            Category = category;
        }

        public Type ProviderType { get; }
        public string? ProviderAlias { get; }
        public string? ProviderTypeFullName { get; }

        public IEmailSender EmailSender { get; }

        public string Category { get; }
    }
}
