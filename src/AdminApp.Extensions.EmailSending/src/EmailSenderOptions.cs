// Use of this source code is governed by an MIT-style license that can be found in the LICENSE.txt file at the repository root.

namespace AdminApp.Extensions.EmailSending
{
    public class EmailSenderOptions
    {
        public IList<string> PreferredProviderNames => InternalPreferredProviderNames;
        public IList<CategoryEmailRule> CategoryEmailRules => InternalCategoryEmailRules;
        public List<FromEmailProviderNameRule> FromEmailProviderNameRules => InternalFromEmailProviderNameRules;

        internal List<string> InternalPreferredProviderNames { get; } = new List<string>();
        internal List<CategoryEmailRule> InternalCategoryEmailRules { get; } = new List<CategoryEmailRule>();
        internal List<FromEmailProviderNameRule> InternalFromEmailProviderNameRules { get; } = new List<FromEmailProviderNameRule>();
    }
}
