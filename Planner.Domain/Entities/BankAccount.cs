using Planner.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planner.Domain.Entities
{
    public class BankAccount:BaseEntity
    {
     
        public BankAccount(int bankId, string description,Guid userId, string color = "#000000" , TypeAccount type = TypeAccount.Others, decimal initialBalance = 0):base()
        {
            BankId = bankId;
            Description = description;
            Color = color;
            Type = type;
            UserId = userId;
            InitialBalance = initialBalance;
        }

        public int BankId { get; set; }
        public string Description { get; set; } 
        public string Color { get; set; }
        public TypeAccount Type { get; set; }
        public decimal InitialBalance { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public ICollection<Transaction> Transactions { get; set; }

        public IEnumerable<CreditCard> CreditCards { get; set; }

        public void UpdateDescription(string description)
        {
            Description = description;
        }

        public void UpdateColor(string color)
        {
            Color = color;
        }
        public void UpdateType(TypeAccount type)
        {
            Type = type;
        }
        public void UpdateBank(int bankId)
        {
            BankId = bankId;
        }

    }
}
