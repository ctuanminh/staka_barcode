using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Be.Core.BaseEntities;

namespace Be.Core.Entities.Crm
{
    public class TaskList : AuditedEntity
    {
        public string Name { get; set; } = string.Empty;
        public int SortOrder { get; set; }

        public Guid BoardId { get; set; }
        public Board Board { get; set; } = null!;

        public ICollection<Card> Cards { get; set; } = [];
    }
}
