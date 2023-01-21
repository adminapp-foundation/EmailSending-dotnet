// Use of this source code is governed by an MIT-style license that can be found in the LICENSE.txt file at the repository root.

namespace AdminApp.Extensions.EmailSending
{
    public class CategoryEmailRule
    {
        public CategoryEmailRule(string categoryName, EmailAddress fromEmailAddress, EmailAddress? replyToEmailAddress = null)
        {
            CategoryName = categoryName;
            FromEmailAddress = fromEmailAddress;
            ReplyToEmailAddress = replyToEmailAddress;
        }

        public string CategoryName { get; }

        public EmailAddress FromEmailAddress { get; }

        public EmailAddress? ReplyToEmailAddress { get; }
    }
}
