// Use of this source code is governed by an MIT-style license that can be found in the LICENSE.txt file at the repository root.

namespace AdminApp.Extensions.EmailSending
{
    public class FromEmailProviderNameRule
    {
        public FromEmailProviderNameRule(string fromEmail, IList<string> providerNames)
        {
            FromEmail = fromEmail;
            ProviderNames = providerNames;
        }

        public string FromEmail { get; }
        public IList<string> ProviderNames { get; }
    }
}
