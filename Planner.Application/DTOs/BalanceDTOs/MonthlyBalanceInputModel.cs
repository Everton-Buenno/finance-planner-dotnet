using System;
using System.ComponentModel.DataAnnotations;

namespace Planner.Application.DTOs.BalanceDTOs
{
    public class MonthlyBalanceInputModel
    {
        [Required(ErrorMessage = "UserId é obrigatório")]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "Ano é obrigatório")]
        [Range(2000, 3000, ErrorMessage = "Ano deve estar entre 2000 e 3000")]
        public int Year { get; set; }

        [Required(ErrorMessage = "Mês é obrigatório")]
        [Range(1, 12, ErrorMessage = "Mês deve estar entre 1 e 12")]
        public int Month { get; set; }

        public Guid? BankAccountId { get; set; }
    }
}