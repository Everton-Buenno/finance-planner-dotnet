using Planner.Domain.Enums;

namespace Planner.Domain.Entities
{
    public class Category : BaseEntity {
        public Category(string name, string color, string icon, Guid userId, TypeCategory type) : base()
        {
            Name = name;
            Color = color;
            Icon = icon;
            UserId = userId;
            Type = type;
        }

        public string Name { get; set; }
        public string Color { get; set; }
        public string Icon { get; set; }

        public TypeCategory Type { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public IEnumerable<Transaction> Transactions { get; set; }


        public void UpdateCategory(string name, string color, string icon)
        {
            Name = name;
            Color = color;
            Icon = icon;
            MarkAsUpdated();
        }


    }


}
