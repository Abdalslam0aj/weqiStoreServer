using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace weqi_store_api.Models.Entities
{
    public class ProductImage
    {
        [Required]
        public string imageId { set; get; }
        public string url { set; get; }
        public string productId {set;get;}
        [ForeignKey(nameof(productId))]
        public Product Product { set; get; }


   
 
        public ProductImage(string imageId, string url, string productId)
        {
            this.imageId = createImgId();
            this.productId = productId;
            this.url = url;
        }

        private string createImgId()
        {
           
            var date = DateTime.UtcNow;
            var firstHalf = date.Year.ToString() + date.Month.ToString() + date.Day.ToString();
            var secondeHalf = GetRandomString();
            return firstHalf + secondeHalf + date.Minute.ToString();

        }

        private static string GetRandomString()
        {
            string path = Path.GetRandomFileName();
            path = path.Replace(".", "");
            return path;
        }
        public static ProductImage SaveImage(string ImgStr, string productId)
        {
            var savedImage = new ProductImage(null, "", productId);

            String path = Path.Combine(Directory.GetCurrentDirectory(), "/projects/weqi_proj/backend/weqi_store_api/weqi_store_api/ImageStorage/"+productId);

            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path); 
            }

            string imageName = savedImage.imageId + ".jpg";
          
            string imgPath = Path.Combine(path, imageName);

            byte[] imageBytes = Convert.FromBase64String(ImgStr);

            File.WriteAllBytes(imgPath, imageBytes);

            savedImage.url =productId + "/" + imageName;
           

            return savedImage;
        }

    }
}
