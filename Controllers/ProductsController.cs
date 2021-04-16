using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using weqi_store_api.Data;
using weqi_store_api.Models.DTOs.Requests;
using weqi_store_api.Models.DTOs.Responses;
using weqi_store_api.Models.Entities;

namespace weqi_store_api.Controllers
{
    // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly WeqiDbContext _weqiDbContext;

        public ProductsController(WeqiDbContext weqiDbContext)
        {
            _weqiDbContext = weqiDbContext;

        }

        [HttpGet]
        [Route("GetProducts")]
        public IActionResult GetProducts(string search) {
            List<GetProductsResponse> getProducts = new List<GetProductsResponse>(){};
            var products = _weqiDbContext.Products.ToList();
            products.ForEach(p => {
                GetProductsResponse product = new GetProductsResponse();
                product.product = p;
                product.ProductImages = _weqiDbContext.ProductImages.Where(e => e.productId == p.Id).ToList();
                getProducts.Add(product);
            });
            
           
            if (getProducts.Count == 0)
            {
                return Ok("NO DATA");
            }
            return Ok(getProducts);
        }

        [HttpPost]
        [Route("AddProduct")]
        public async Task<IActionResult> AddProduct([FromBody] PostProductRequest product)
        {
            Product savedProduct;
            List<ProductImage> productImages = new List<ProductImage>() { };            
           
            try {
                savedProduct =  _weqiDbContext.Products.Add(product.product).Entity;
                product.base64Images.ForEach(i => {                    
                   var savedProductImage =  ProductImage.SaveImage(i,savedProduct.Id);
                    productImages.Add(savedProductImage);
                });
               
                await  _weqiDbContext.ProductImages.AddRangeAsync(productImages);
                _weqiDbContext.SaveChanges();

            } catch(Exception e) {
                return BadRequest(e);            
             
            }
            
            return Ok(savedProduct);
        }
    }
}
