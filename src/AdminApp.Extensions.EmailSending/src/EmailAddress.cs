// Use of this source code is governed by an MIT-style license that can be found in the LICENSE.txt file at the repository root.

namespace AdminApp.Extensions.EmailSending
{
    public class EmailAddress
    {
        public EmailAddress(string email, string? name = null)
        {
            Email = email;
            Name = name;
        }

        public string Email { get; set; }
        public string? Name { get; set; }
    }
}
