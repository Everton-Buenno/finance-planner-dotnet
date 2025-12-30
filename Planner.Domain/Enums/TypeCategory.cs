using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planner.Domain.Enums
{
    public enum TypeCategory
    {
        [Description("Receita")]
        Income = 1,
        [Description("Despesa")]
        Expense = 2,
    }
}
