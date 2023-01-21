// Use of this source code is governed by an MIT-style license that can be found in the LICENSE.txt file at the repository root.

namespace AdminApp.Extensions.EmailSending
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class EmailSenderProviderAliasAttribute : Attribute
    {
        public EmailSenderProviderAliasAttribute(string alias) { }

#pragma warning disable CS8597 // Thrown value may be null.
        public string Alias => throw null;
#pragma warning restore CS8597 // Thrown value may be null.
    }
}
