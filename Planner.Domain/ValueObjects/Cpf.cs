using Planner.Domain.Exceptions;
using System.Text.RegularExpressions;

namespace Planner.Domain.ValueObjects
{
    public class Cpf
    {
        public string Value { get; }
        public Cpf(string value)
        {
            if(string.IsNullOrWhiteSpace(value))
                throw new DomainException("CPF cannot be empty");

            if(!IsValidCpf(value))
                throw new DomainException("Invalid CPF format");

            Value = Regex.Replace(value, @"\D", "");
        }

        public override string ToString() => Value;
        public string Formatted() => Convert.ToUInt64(Value).ToString(@"000\.000\.000\-00");

        private bool IsValidCpf(string cpf)
        {
            cpf = Regex.Replace(cpf, @"\D", "");

            if (cpf.Length != 11) return false;

            if (new string(cpf[0], cpf.Length) == cpf) return false;

            int[] multiplicador1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCpf = cpf.Substring(0, 9);
            int soma = 0;

            for (int i = 0; i < 9; i++)
                soma += (tempCpf[i] - '0') * multiplicador1[i];

            int resto = soma % 11;
            int primeiroDigito = resto < 2 ? 0 : 11 - resto;

            tempCpf += primeiroDigito;
            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += (tempCpf[i] - '0') * multiplicador2[i];

            resto = soma % 11;
            int segundoDigito = resto < 2 ? 0 : 11 - resto;

            return cpf.EndsWith($"{primeiroDigito}{segundoDigito}");
        }
    }
}
