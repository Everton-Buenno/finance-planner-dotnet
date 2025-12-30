using System.ComponentModel;

namespace Planner.Domain.Enums
{
    public enum TypeAccount
    {
        [Description("Conta Corrente")]
        CheckingAccount = 0,

        [Description("Dinheiro")]
        Cash = 1,

        [Description("Poupança")]
        Savings = 2,

        [Description("Investimentos")]
        Investments = 3,

        [Description("VR/VA")]
        FoodVoucher = 4,

        [Description("Outros")]
        Others = 5
    }
}
