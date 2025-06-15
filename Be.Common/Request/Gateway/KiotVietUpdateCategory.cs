using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Be.Common.Request.Gateway
{
    public class KiotVietUpdateCategory
    {
        public string Id { get; set; }
        public int Attempt { get; set; }
        public List<CategoryUpdateNotification> Notifications { get; set; }
    }
    public class CategoryUpdateNotification
    {
        public string Action { get; set; }
        public List<CategoryUpdateData> Data { get; set; }
    }

    public class CategoryUpdateData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int RetailerId { get; set; }
        public int Rank { get; set; }
        public bool HasChild { get; set; }
    }
}
