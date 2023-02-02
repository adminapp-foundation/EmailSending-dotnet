// Use of this source code is governed by an MIT-style license that can be found in the LICENSE.txt file at the repository root.

namespace AdminApp.Extensions.EmailSending
{
    public class EmailSendingOptions
    {
        /// <summary>
        /// Default email provider orders.
        /// </summary>
        public IList<string> ProviderOrders { get; } = new List<string>();

        /// <summary>
        /// Category name and From email rules.
        /// </summary>
        public IDictionary<string, CategoryRuleValue> CategoryRules { get; } = new Dictionary<string, CategoryRuleValue>();

        /// <summary>
        /// From email and provider names rules.
        /// </summary>
        public IDictionary<string, List<string>> FromRules { get; } = new Dictionary<string, List<string>>();
    }

    public class CategoryRuleValue
    {
        public CategoryRuleValue(EmailAddress fromEmailAddress, EmailAddress? replyToEmailAddress = null)
        {
            FromEmailAddress = fromEmailAddress;
            ReplyToEmailAddress = replyToEmailAddress;
        }

        public EmailAddress FromEmailAddress { get; set; }
        public EmailAddress? ReplyToEmailAddress { get; set; }
    }
}
