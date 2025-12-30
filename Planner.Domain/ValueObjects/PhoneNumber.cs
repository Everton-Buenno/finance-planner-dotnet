using Planner.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Planner.Domain.ValueObjects
{
    public class PhoneNumber
    {
        public string CountryCode { get; }
        public string Number { get; }

        public PhoneNumber(string countryCode, string number)
        {
            if(string.IsNullOrWhiteSpace(number))
                throw new DomainException("Phone number cannot be empty");

            if(!IsValidPhoneNumber(number))
                throw new DomainException("Phone number is invalid");

            CountryCode = countryCode ?? "55";
            Number = number;
        }

        private bool IsValidPhoneNumber(string number)
        {
            return Regex.IsMatch(number, @"^\d{8,15}$");
        }

        public override string ToString()
        {
            return $"+{CountryCode}{Number}";
        }
    }
}
