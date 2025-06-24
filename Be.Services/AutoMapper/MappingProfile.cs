using AutoMapper;
using Be.Common.Branch.Response;
using Be.Common.Dtos.Category;
using Be.Common.Dtos.Product;
using Be.Common.Supply.Dto;
using Be.Common.Supply.Response;
using Be.Common.System;
using Be.Core.Entities;

namespace Be.Services.AutoMapper
{
	public class MappingProfile : Profile
	{
        public MappingProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Category, CategoryDto>().ReverseMap();
            //Map AppSetting và AppSettingDto
            CreateMap<AppSettingEntity, AppSettingDto>().ReverseMap();
            CreateMap<BranchResponse, Branch>().ReverseMap();
            CreateMap<SupplierDto, SupplierEntity>().ReverseMap();
            CreateMap<SupplierResponse, SupplierEntity>().ReverseMap();
        }
    }
}
