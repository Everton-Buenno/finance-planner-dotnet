using Planner.Domain.Enums;
using Planner.Domain.Exceptions;
using Planner.Domain.ValueObjects;


namespace Planner.Domain.Entities
{
    public class User : BaseEntity
    {
        private User() { }
        public User(string name, Email email, string passwordHash, Address address = null, bool darkMode = false) : base()
        {
            Name = name;
            Email = email;
            PasswordHash = passwordHash;
            Address = address;
            DarkMode = darkMode;
            Plan = PlanType.Free;
        }

        public string Name { get; private set; }
        public Email Email { get; private set; }
        public string PasswordHash { get; private set; }
        public Gender? Gender { get; private set; }
        public string? Nationality { get; private set; }
        public PhoneNumber? PhoneNumber { get; private set; }
        public Address Address { get; private set; }
        public DateTime? LastLogin { get; private set; }
        public Cpf? Cpf { get; private set; }
        public bool DarkMode { get; private set; }
        public PlanType Plan { get; private set; }
        public IEnumerable<BankAccount> Accounts { get; private set; }
        public IEnumerable<Transaction> Transactions { get; private set; }
        public IEnumerable<Category> Categories { get; private set; }

        #region Methods
        public void UpdateDarkMode(bool darkMode)
        {
            DarkMode= darkMode;
            MarkAsUpdated();
        }
        public void UpdatePlan(PlanType plan)
        {
            Plan = plan;
            MarkAsUpdated();
        }
        public void UpdateCpf(Cpf cpf)
        {
            Cpf = cpf;
            MarkAsUpdated();
        }
        public void UpdateNationality(string nationality)
        {
            Nationality = nationality;
            MarkAsUpdated();
        }
        public void UpdateGender(Gender gender)
        {
            Gender = gender;
            MarkAsUpdated();
        }

        public void UpdatePhoneNumber(PhoneNumber phoneNumber)
        {
            PhoneNumber = phoneNumber;
            MarkAsUpdated();
        }

        public void UpdateAddress(Address address)
        {
            Address = address;
            MarkAsUpdated();
        }

        public void UpdatePassword(string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(passwordHash))
            {
                throw new DomainException("Password cannot be empty");
            }

            PasswordHash = passwordHash;
            MarkAsUpdated();
        }
        public void UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Name cannot be empty");

            Name = name;
            MarkAsUpdated();
        }
        public  void RecordLogin()
        {
            LastLogin = DateTime.UtcNow;
        }

        #endregion
    }

}
