using AutoMapper;
using Sales.Application.DTOs.Product;
using Sales.Application.DTOs.Order;
using Sales.Application.DTOs.Category;
using Sales.Application.DTOs.Unit;
using Sales.Application.DTOs.Inventory.GoodsReceipt;
using Sales.Domain.Entities.Product;
using Sales.Domain.Entities.Order;

namespace Sales.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // GoodsReceipt mappings
            CreateMap<Sales.Domain.Entities.Inventory.GoodsReceipt, GoodsReceiptDto>();
            CreateMap<Sales.Domain.Entities.Inventory.GoodsReceipt, GoodsReceiptSummaryDto>();
            CreateMap<CreateGoodsReceiptDto, Sales.Domain.Entities.Inventory.GoodsReceipt>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.TotalQuantity, opt => opt.Ignore());

            CreateMap<Sales.Domain.Entities.Inventory.GoodsReceiptLine, GoodsReceiptLineDto>();
            CreateMap<CreateGoodsReceiptLineDto, Sales.Domain.Entities.Inventory.GoodsReceiptLine>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore());

            // Product mappings
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.BaseUnitName, opt => opt.MapFrom(src => src.BaseUnit.Name))
                .ForMember(dest => dest.Conversions, opt => opt.MapFrom(src => src.Conversions));
            
            CreateMap<CreateProductDto, Product>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.StockQuantity, opt => opt.Ignore())
                .ForMember(dest => dest.Conversions, opt => opt.Ignore());

            CreateMap<ProductUnitConversion, ProductUnitConversionDto>()
                .ForMember(dest => dest.AlternativeUnitName, opt => opt.MapFrom(src => src.AlternativeUnit.Name));

            CreateMap<ProductUnitConversionDto, ProductUnitConversion>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()));

            // Order mappings
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.Name));
            
            CreateMap<OrderDetail, OrderDetailDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.ProductCode, opt => opt.MapFrom(src => src.Product.Code))
                .ForMember(dest => dest.UnitName, opt => opt.MapFrom(src => src.Unit.Name));

            // Category mappings
            CreateMap<Category, CategoryDto>();
            CreateMap<CreateCategoryDto, Category>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore());

            // Unit mappings
            CreateMap<Unit, UnitDto>();
            CreateMap<CreateUnitDto, Unit>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore());
        }
    }
}
