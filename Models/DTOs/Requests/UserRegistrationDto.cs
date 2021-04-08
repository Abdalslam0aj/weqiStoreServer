using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace weqi_store_api.Models.DTOs.Requests
{
    public class UserRegistrationDto
    {
        [Required]
        [Phone]
        public string phoneNumber { get; set; }
        [Required]        
        public string userName { get; set; }
        [Required]
        public string password { get; set; }

    }
}
