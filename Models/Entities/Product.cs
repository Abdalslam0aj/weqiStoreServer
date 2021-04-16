using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace weqi_store_api.Models.Entities
{
    public class Product
    {

        [Key]
        public string Id
        {
            set;
            get;
        }
        [Required]
        public string name { set; get; }
        [Required]
        public double price { set; get; }
        [Required]
        public string description { set; get; }
        [Required]
        public double sale { set; get; }
        public string videoUrl { set; get; }
       // public List<string> features { set;get;}
        // public List<ProductImage> images { set; get; } List<ProductImage> images
        // public List<ProductReviews> reviews { set; get; } 
        // public List<ProductTags> tags { set; get;}
        //
        // TODO: add to constractar
        public Product(string name, double price, string description, double sale )// List<string> features ) 
        {
            Id = createProductId(name);
            this.name = name;
            this.price = price;
            this.description = description;
            this.sale = sale;
          //  this.features = features;
            //this.images = images;     
        
        }

        private string createProductId (string name)
        {
            var lastHalf = "PROD";
            if (name != null) {
                lastHalf = name.Replace(' ','%').Trim();            
            }
            var date =  DateTime.UtcNow;
            var firstHalf = date.Year.ToString() + date.Month.ToString() + date.Day.ToString();
            var secondeHalf = GetRandomString();           
            return firstHalf + secondeHalf + lastHalf;

        }
        private static string GetRandomString()
        {
            string path = Path.GetRandomFileName();
            path = path.Replace(".", "");
            return path;
        }


    }
}
