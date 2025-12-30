using Planner.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planner.Domain.ValueObjects
{
    public class Email
    {
        public string Value { get; }

        public Email(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainException("Email cannot be empty");

            if (!IsValidEmail(value))
                throw new DomainException("Invalid email format");

            Value = value;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public override string ToString() => Value;

        public override bool Equals(object obj)
        {
            if (obj is Email other)
                return Value.Equals(other.Value, StringComparison.OrdinalIgnoreCase);
            return false;
        }

        public override int GetHashCode()
        {
            return Value.ToLowerInvariant().GetHashCode();
        }

        public static bool operator ==(Email a, Email b)
        {
            if (ReferenceEquals(a, b)) return true;
            if (a is null || b is null) return false;
            return a.Value.Equals(b.Value, StringComparison.OrdinalIgnoreCase);
        }

        public static bool operator !=(Email a, Email b) => !(a == b);
    }
}
