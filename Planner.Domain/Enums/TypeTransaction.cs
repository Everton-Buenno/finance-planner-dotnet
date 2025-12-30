using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planner.Domain.Enums
{
    public enum TypeTransaction
    {
        [Description("Receita")]
        Income = 0,

        [Description("Despesa")]
        Expense = 1,

        [Description("Despesa de Cartão de Crédito")]
        CreditCardExpense = 2,

        [Description("Transferência")]
        Transfer = 3

    }
}
