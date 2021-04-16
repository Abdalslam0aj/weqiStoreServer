using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using weqi_store_api.Models.Entities;

namespace weqi_store_api.Models.DTOs.Requests
{
    public class PostProductRequest
    {
        public Product product { set; get; }
        public List<string> base64Images { set; get; }

    }
}
