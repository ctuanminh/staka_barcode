using Be.Core.BaseEntities;

namespace Be.Core.Entities
{
    public class Category : AuditedEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public List<Product> Products { get; set; }
    }

}

