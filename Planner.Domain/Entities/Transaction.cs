using Planner.Domain.Enums;
using Planner.Domain.Exceptions;
using System.Drawing;
using System.Xml.Linq;

namespace Planner.Domain.Entities
{
    public class Transaction:BaseEntity
    {
        public Transaction() { }
        public Transaction(DateTime dateTransaction, 
            decimal value,
            string description, 
            TypeTransaction type, 
            Guid accountId,  
            Guid? categoryId,
            bool isPaid = false , 
            bool ignored = false,
            Guid? transactionOrigemId = null,
            RepeatType? repeatType = Enums.RepeatType.None,
            Guid? creditCardId = null
           )
        {
            DateTransaction = dateTransaction;
            Value = value;
            Description = description;
            Type = type;
            AccountId = accountId;
            CategoryId = categoryId;
            IsPaid = isPaid;
            Ignored = ignored;
            RepeatType = repeatType;
            TransactionOrigemId = transactionOrigemId;
            CreditCardId = creditCardId;
        }

        public DateTime DateTransaction { get; set; }
        public decimal Value { get; set; }
        public string Description { get; set; }
        public TypeTransaction Type { get; set; }
        public Guid AccountId { get; set; }
        public BankAccount Account { get; set; }
        public Guid? CategoryId { get; set; }
        public Category Category { get; set; }

        public bool IsPaid { get; set; } 
        public bool Ignored { get; set; } 
        public RepeatType? RepeatType { get; set; } 
        public Guid? TransactionOrigemId { get; set; } 
        public Transaction? TransactionOrigem { get; set; }
        public ICollection<Transaction>? Transactions { get; set; }

        public Guid? CreditCardId { get; set; }
        
        public CreditCard? CreditCard { get; set; }


        public void UpdateTransaction(DateTime dateTransaction,decimal value ,string description, bool isPaid, bool ignored, TypeTransaction type, Guid accountId, Guid categoryId, Guid? creditCardId = null)
        {

            DateTransaction = dateTransaction;
            Value = value;
            Description = description;
            IsPaid = isPaid;
            Ignored = ignored;
            Type = type;
            AccountId = accountId;
            CategoryId = categoryId;
            CreditCardId = creditCardId;
            MarkAsUpdated();
        }

    
    }
}
