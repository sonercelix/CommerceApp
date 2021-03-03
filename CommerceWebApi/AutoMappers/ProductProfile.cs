using AutoMapper;
using CommerceWebApi.Entities;
using CommerceWebApi.Models;

namespace CommerceWebApi.AutoMappers
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductModel>();
            CreateMap<ProductModel, Product>();


            CreateMap<spProductItem, ProductModel>();
            CreateMap<ProductModel, spProductItem>();
            CreateMap<CategoryModel, Category>();
            CreateMap<Category, CategoryModel>();
        }
    }
}