using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestApi.Dtos.ProductDtos;
using TestApi.Models.Response;

namespace TestApi.Services.ProductService
{
    public interface IProductService
    {
        Task<ServiceResponse<List<GetProductDto>>> GetProducts(int Page, int Limit, string Code, string Name );
        Task<ServiceResponse<GetProductDto>> GetProductById(int Id);
        Task<ServiceResponse<GetProductDto>> AddProduct(AddProductDto request);
        Task<ServiceResponse<GetProductDto>> UpdateProduct(UpdateProductDto request);
        Task<ServiceResponse<GetProductDto>> DeleteProduct(int Id);
        
    }
}