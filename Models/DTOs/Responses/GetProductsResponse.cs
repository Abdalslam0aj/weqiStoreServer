using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using weqi_store_api.Models.Entities;

namespace weqi_store_api.Models.DTOs.Responses
{
    public class GetProductsResponse
    {
        public Product product { set; get; }
        public List<ProductImage> ProductImages { set; get; }


      // public List<string> features { set; get; }      
      //  public List<ProductReviews> reviews { set; get; }
      //  public List<ProductTags> tags { set; get; }


    }
    
}
