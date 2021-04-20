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
        private string id; // field

        public string Id   // property
        {
            get { return id; }   // get method
            set { id = value; }  // set method
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
        public virtual ICollection<ProductImage> images { set; get; }
        // public List<ProductReviews> reviews { set; get; } 
        // public List<ProductTags> tags { set; get;}
        //
        // TODO: add to constractar
        public Product() { 
        }
        public Product(string name, double price, string description, double sale, string videoUrl, List<ProductImage> images)// List<string> features ) 
        {
            Id = createProductId(name);
            this.name = name;
            this.price = price;
            this.description = description;
            this.sale = sale;
            this.videoUrl = videoUrl;
          //  this.features = features;
            this.images = images;     
        
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
