using System.ComponentModel;

namespace Planner.Domain.Enums
{
    public enum Gender
    {
        [Description("Não especificado")]
        NaoEspecificado = 0,

        [Description("Masculino")]
        Masculino = 1,

        [Description("Feminino")]
        Feminino = 2,

        [Description("Outro")]
        Outro = 3
    }
}
