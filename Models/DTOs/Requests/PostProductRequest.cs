using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using weqi_store_api.Models.Entities;

namespace weqi_store_api.Models.DTOs.Requests
{
    public class PostProductRequest
    {
          
        [Required]
        public string name { set; get; }
        [Required]
        public double price { set; get; }
        [Required]
        public string description { set; get; }
        [Required]
        public double sale { set; get; }
        public string videoUrl { set; get; }
        public List<string> base64Images { set; get; }

    }
}
