using Planner.Domain.Entities;
using Planner.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planner.Application.DTOs.CategoryDTOs
{
    public class CategoryInputModel
    {
        public string Name { get; set; }
        public string Color { get; set; }
        public string Icon { get; set; }
        public TypeCategory Type { get; set; }
        public Guid UserId { get; set; }
    }
}
