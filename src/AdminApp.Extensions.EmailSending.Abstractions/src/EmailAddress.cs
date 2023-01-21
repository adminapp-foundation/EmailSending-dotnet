// Use of this source code is governed by an MIT-style license that can be found in the LICENSE.txt file at the repository root.

namespace AdminApp.Extensions.EmailSending
{
    public class EmailAddress : IEquatable<EmailAddress>
    {
        public EmailAddress(string email, string? name = null)
        {
            Email = email;
            Name = name;
        }

        public string Email { get; set; }
        public string? Name { get; set; }

        public static bool operator ==(EmailAddress left, EmailAddress right)
        {
            if ((object)left == null && (object)right == null)
            {
                return true;
            }

            return left?.Equals(right) ?? false;
        }

        public static bool operator !=(EmailAddress left, EmailAddress right)
        {
            return !(left == right);
        }

        public bool Equals(EmailAddress? other)
        {
            if (other is null)
            {
                return false;
            }

            if ((object)this == other)
            {
                return true;
            }

            return string.Equals(Email, other.Email, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (this == (EmailAddress)obj)
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((EmailAddress)obj);
        }

        public override int GetHashCode()
        {
            if (Email == null)
            {
                return 0;
            }

            return Email.ToLower().GetHashCode();
        }
    }
}
