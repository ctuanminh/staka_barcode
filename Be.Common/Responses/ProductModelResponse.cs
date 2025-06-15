using Be.Common.Dtos.Category;

namespace Be.Common.Responses
{
    public class ProductModelResponse
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Content { get; set; }
        public Double Price { get; set; }
        public int NumOfProduct { get; set; }
        public long CategoryId { get; set; }
        public bool Status { get; set; }
        public CategoryDto Category { get; set; }
    }
}
