using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planner.Domain.Entities
{
    public abstract class BaseEntity
    {
        protected BaseEntity()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            IsDeleted = false;
        }

        public Guid Id { get; private set; }
        public DateTime CreatedAt { get;  set; }
        public DateTime? DeletedAt { get;  set; }
        public DateTime? UpdatedAt { get;  set; }
        public bool IsDeleted { get;  set; }

        public void Delete()
        {
            DeletedAt = DateTime.UtcNow;
            IsDeleted = true;
        }
        public void MarkAsUpdated() 
        {
            UpdatedAt = DateTime.UtcNow;    
        }
    }
}
