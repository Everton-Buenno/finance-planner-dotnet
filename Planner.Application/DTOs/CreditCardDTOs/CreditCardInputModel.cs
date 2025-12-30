using System;
using System.ComponentModel.DataAnnotations;

namespace Planner.Application.DTOs.CreditCardDTOs
{
    public class CreditCardInputModel
    {
        [Required(ErrorMessage = "Descrição é obrigatória")]
        [StringLength(100, ErrorMessage = "Descrição deve ter no máximo 100 caracteres")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Limite de crédito é obrigatório")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Limite de crédito deve ser maior que zero")]
        public decimal CreditLimit { get; set; }

        [Required(ErrorMessage = "Dia de vencimento é obrigatório")]
        [Range(1, 31, ErrorMessage = "Dia de vencimento deve estar entre 1 e 31")]
        public int DueDay { get; set; }

        [Required(ErrorMessage = "Dia de fechamento é obrigatório")]
        [Range(1, 31, ErrorMessage = "Dia de fechamento deve estar entre 1 e 31")]
        public int ClosingDay { get; set; }

        [Required(ErrorMessage = "Conta bancária é obrigatória")]
        public Guid AccountId { get; set; }
    }
}