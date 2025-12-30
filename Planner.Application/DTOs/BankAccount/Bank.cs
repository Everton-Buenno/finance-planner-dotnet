using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Planner.Application.DTOs.BankAccount
{
    public class Bank
    {
        [JsonPropertyName("bankId")]
        public int BankId { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("pathImage")]
        public string PathImage { get; set; }
    }
}
