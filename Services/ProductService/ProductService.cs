using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TestApi.Data;
using TestApi.Dtos.ProductDtos;
using TestApi.Models.DB;
using TestApi.Models.Response;
using TestApi.Services.ValidatorService;

namespace TestApi.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly ILogger<ProductService> _logger;
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IValidatorService _validator;

        public ProductService(ILogger<ProductService> logger, DataContext context, IMapper mapper, IHttpContextAccessor httpContext, IValidatorService validator)
        {
            _validator = validator;
            _httpContext = httpContext;
            _mapper = mapper;
            _context = context;
            _logger = logger;

        }
        public async Task<ServiceResponse<List<GetProductDto>>> GetProducts(int Page, int Limit, string Code, string Name)
        {
            var serviceResponse = new ServiceResponse<List<GetProductDto>>();
            var validatorMessage = _validator.PageValidation(Page, Limit);
            if (validatorMessage != "")
            {
                serviceResponse.Message = validatorMessage;
                return serviceResponse;
            }

            try
            {
                var page = Page;
                var limit = Limit;
                var skip = (page - 1) * limit;

                var query = await _context.Products
                    .Where(c => (c.Code.Equals(Code) || Code == "")
                     && (c.Name.Contains(Name) || Name == "")).ToListAsync();

                var itemCount = query.Count();
                if (itemCount == 0) itemCount = 1;

                var Product = query.OrderBy(c => c.Code)
                    .Skip(skip)
                    .Take(limit)
                    .Select(c => _mapper.Map<GetProductDto>(c))
                    .ToList();

                serviceResponse.Data = Product;
                serviceResponse.Page.CurrentPage = page;
                serviceResponse.Page.PageCount = System.Convert.ToInt32(System.Math.Ceiling(itemCount / System.Convert.ToDouble(limit)));
                serviceResponse.Success = true;
                serviceResponse.Code = "000";
            }
            catch (Exception e)
            {

                serviceResponse.Message = e.Message;
                _logger.LogError(e, "Exception caught.");
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetProductDto>> GetProductById(int Id)
        {
            var serviceResponse = new ServiceResponse<GetProductDto>();
            try
            {
                var product = await _context.Products
                    .Where(c => c.Id.Equals(Id))
                    .Select(c => _mapper.Map<GetProductDto>(c)).FirstOrDefaultAsync();

                serviceResponse.Data = product;
                serviceResponse.Success = true;
                serviceResponse.Code = "000";
            }
            catch (Exception e)
            {

                serviceResponse.Message = e.Message;
                _logger.LogError(e, "Exception caught.");
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetProductDto>> AddProduct(AddProductDto request)
        {
            var serviceResponse = new ServiceResponse<GetProductDto>();
            try
            {
                var product = new Product
                {
                    Code = request.Code,
                    Name = request.Name,
                    ItemNo = request.ItemNo,
                    Price = request.Price,
                    Quantity = request.Quantity,
                    ImageUrl = request.ImageUrl,
                    BarCode = request.BarCode,
                    Remark = request.Remark,
                    CreageBy = "Admin",
                    CteateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                };
                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                serviceResponse.Data = _mapper.Map<GetProductDto>(product);
                serviceResponse.Success = true;
                serviceResponse.Code = "000";
            }
            catch (Exception e)
            {

                serviceResponse.Message = e.Message;
                _logger.LogError(e, "Exception caught.");
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetProductDto>> UpdateProduct(UpdateProductDto request)
        {
            var serviceResponse = new ServiceResponse<GetProductDto>();
            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(c => c.Id.Equals(request.Id));
                if (product == null)
                {
                    serviceResponse.Message = $"The product id is not valid.";
                    serviceResponse.Code = "100";
                    return serviceResponse;
                }
                product.Code = request.Code;
                product.Name = request.Name;
                product.ItemNo = request.ItemNo;
                product.Price = request.Price;
                product.Quantity = request.Quantity;
                product.ImageUrl = request.ImageUrl;
                product.BarCode = request.BarCode;
                product.Remark = request.Remark;
                product.UpdateBy = "Admin";
                product.UpdateDate = DateTime.Now;

                await _context.SaveChangesAsync();

                serviceResponse.Data = _mapper.Map<GetProductDto>(product);
                serviceResponse.Success = true;
                serviceResponse.Code = "000";
            }
            catch (Exception e)
            {

                serviceResponse.Message = e.Message;
                _logger.LogError(e, "Exception caught.");
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetProductDto>> DeleteProduct(int Id)
        {
            var serviceResponse = new ServiceResponse<GetProductDto>();
            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(c => c.Id.Equals(Id));
                if (product == null)
                {
                    serviceResponse.Message = $"The product id is not valid.";
                    serviceResponse.Code = "100";
                    return serviceResponse;
                }
                _context.Remove(product);
                await _context.SaveChangesAsync();

                // serviceResponse.Data = product;
                serviceResponse.Success = true;
                serviceResponse.Code = "000";
            }
            catch (Exception e)
            {

                serviceResponse.Message = e.Message;
                _logger.LogError(e, "Exception caught.");
            }
            return serviceResponse;
        }
    }
}