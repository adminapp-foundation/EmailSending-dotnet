// Use of this source code is governed by an MIT-style license that can be found in the LICENSE.txt file at the repository root.

using System.Diagnostics;
using System.Reflection;

namespace AdminApp.Extensions.EmailSending
{
    internal static class ProviderAliasUtilities
    {
        // private const string AliasAttributeTypeFullName = "AdminApp.Extensions.EmailSending.EmailSenderProviderAliasAttribute";

        internal static string? GetAlias(Type providerType)
        {
            IList<CustomAttributeData> attributes = CustomAttributeData.GetCustomAttributes(providerType);

            for (int i = 0; i < attributes.Count; i++)
            {
                CustomAttributeData attributeData = attributes[i];
                // AliasAttributeTypeFullName
                if (attributeData.AttributeType.FullName == typeof(EmailSenderProviderAliasAttribute).FullName &&
                    attributeData.ConstructorArguments.Count > 0)
                {
                    CustomAttributeTypedArgument arg = attributeData.ConstructorArguments[0];

                    Debug.Assert(arg.ArgumentType == typeof(string));

                    return arg.Value?.ToString();
                }
            }

            return null;
        }
    }
}
