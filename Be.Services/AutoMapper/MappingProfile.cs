using AutoMapper;
using Be.Common.Dtos.Category;
using Be.Common.Dtos.Product;
using Be.Core.Entities;

namespace Be.Services.AutoMapper
{
	public class MappingProfile : Profile
	{
        public MappingProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Category, CategoryDto>().ReverseMap();
        }
    }
}
