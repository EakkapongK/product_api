using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestApi.Dtos.ProductDtos;
using TestApi.Models.Response;
using TestApi.Services.ProductService;

namespace TestApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {        
        private readonly IProductService _ProductService;
        public ProductController(IProductService ProductService)
        {
            _ProductService = ProductService;
        }

        [HttpGet("Products")]
        public async Task<ActionResult<ServiceResponse<List<GetProductDto>>>> GetProducts(int Page = 1, int Limit = 10, string Code = "", string Name = "")
        {
            var response = await _ProductService.GetProducts(Page, Limit, Code, Name);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            
            return Ok(response);
        }
        
        [HttpGet("{Id}")]
        public async Task<ActionResult<ServiceResponse<GetProductDto>>> GetProductById(int Id)
        {
            var response = await _ProductService.GetProductById(Id);
            if (!response.Success)
            {
                return BadRequest(response);
            }            
            return Ok(response);
        }
        
        [HttpPost("Add")]
        public async Task<ActionResult<ServiceResponse<GetProductDto>>> AddProduct(AddProductDto request)
        {
            var response = await _ProductService.AddProduct(request);
            if (!response.Success)
            {
                return BadRequest(response);
            }            
            return Ok(response);
        }

        [HttpPut("update")]
        public async Task<ActionResult<ServiceResponse<GetProductDto>>> UpdateProduct(UpdateProductDto request)
        {
            var response = await _ProductService.UpdateProduct(request);
            if (!response.Success)
            {
                return BadRequest(response);
            }            
            return Ok(response);
        }        

        [HttpDelete("Delete/{Id}")]
        public async Task<ActionResult<ServiceResponse<GetProductDto>>> DeleteProduct(int Id)
        {
            var response = await _ProductService.DeleteProduct(Id);
            if (!response.Success)
            {
                return BadRequest(response);
            }            
            return Ok(response);
        }
    }
}