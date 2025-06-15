using Microsoft.AspNetCore.Http;

namespace Be.Common.Dtos.Product
{
    public class ProductCreateRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public float Price { get; set; }
        public List<IFormFile> ItemImages { get; set; }
        public Guid CategoryId { get; set; }
    }    
}
