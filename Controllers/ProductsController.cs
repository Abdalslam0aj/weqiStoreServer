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
using Microsoft.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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
        public IActionResult GetProducts(string search, int page, int size) {
            List<GetProductsResponse> getProducts = new List<GetProductsResponse>() { };
            var products = _weqiDbContext.Products.Include(e => (e).images);
            //if (getProducts.Count == 0)
            //{
            //    return Ok("NO DATA");
            //}
            int boundedPage = 0;
            int boundedSize = 0;

            var bounds = products.ToList().Count;
            if ((page * size) + size <= bounds)
            {
                boundedPage = page * size;
                boundedSize = size;

            }
            else if ((page * size) + size > bounds && bounds > (page * size))
            {
                boundedPage = page * size;
                boundedSize = bounds - boundedPage;

            }
            else {

                return Ok(new List<Product>());
            
            }
            if (search == null) {
                return Ok(products.ToList().GetRange(boundedPage, boundedSize));
            } else {
                Dictionary<string,Product> searchedProducts = new Dictionary<string,Product>();

                products.ToList().ForEach(e =>
                {
                    if (e.name.Contains(search))
                        searchedProducts[e.Id] = e;
                    if(e.name.StartsWith(search))
                        searchedProducts[e.Id] = e;
                    if (e.name.EndsWith(search))
                        searchedProducts[e.Id] = e;

                });
                return Ok(searchedProducts.ToList().GetRange((page * size),size));
            }
        
           
        }

        [HttpPost]
        [Route("AddProduct")]
        public async Task<IActionResult> AddProduct([FromBody] PostProductRequest product)
        {
            List<ProductImage> productImages = new List<ProductImage>() { };
            Product savedProduct = new Product(product.name,product.price,product.description,product.sale,product.videoUrl
                ,productImages);
            
           
            try {
                
                product.base64Images.ForEach(i => {                    
                   var savedProductImage =  ProductImage.SaveImage(i,savedProduct.Id);
                    productImages.Add(savedProductImage);
                });
               // savedProduct.images = productImages;
               savedProduct = _weqiDbContext.Products.Add(savedProduct).Entity;

               await  _weqiDbContext.ProductImages.AddRangeAsync(productImages);
                _weqiDbContext.SaveChanges();

            } catch(Exception e) {
                return BadRequest(e);            
             
            }
            
            return Ok(savedProduct);
        }
    }
}
