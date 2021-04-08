using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using weqi_store_api.Models.DTOs;

namespace weqi_store_api.Data
{
    public class WeqiDbContext : IdentityDbContext
    {
        public virtual DbSet<RefreshTokens> RefreshTokens { get; set; }
        public WeqiDbContext(DbContextOptions<WeqiDbContext> options) : base(options) {

            
        }
    }
}
