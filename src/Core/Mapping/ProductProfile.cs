using AutoMapper;
using Core.DTOs.Products;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mapping
{
    public class ProductProfile:Profile
    {

        public ProductProfile() {

            // => Mapping Product → ProductDto

            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.CategoryName,
                opt =>
                opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.Image,
                opt =>
                opt.MapFrom(src =>  src.Images.Select(i => i.ImageUrl).FirstOrDefault()))
                .ReverseMap();

            // => Mapping Product → ProductDetailsDto
            CreateMap<Product,ProductDetailsDto>()
                 .ForMember(dest => dest.CategoryName,
                opt =>
                opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.SellerName,
                opt =>
                opt.MapFrom(src => src.Seller.Name))
                .ForMember(dest => dest.Images,
                opt =>
                opt.MapFrom(src => src.Images))
                .ReverseMap();

            // => Mapping CreateProductDto → Product
            CreateMap<CreateProductDto, Product>()
                .ForMember(dest => dest.Images,
                           opt => 
                           opt.MapFrom(src =>
                               src.Images.Select(img => new ProductImage
                               {
                                   ImageUrl = img.ImageUrl,
                                   Position = img.Position
                               })
                           ));


            CreateMap<UpdateProductDto, Product>()
                .ForMember(dest => dest.Images,
                           opt =>
                           opt.MapFrom(src =>
                               src.Images.Select(img => new ProductImage
                               {
                                   ImageUrl = img.ImageUrl,
                                   Position = img.Position
                               })
                           ));

            // => Mapping ProductImage → ProductsImagesDto
            CreateMap<ProductImage, ProductsImagesDto>()
                .ForMember(dest => dest.ImageUrl,
                           opt => opt.MapFrom(src => src.ImageUrl))
                .ForMember(dest => dest.Position,
                           opt => opt.MapFrom(src => src.Position))
                .ReverseMap();



        }
    }
}
