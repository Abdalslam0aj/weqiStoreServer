using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using weqi_store_api.Models.DTOs;
using weqi_store_api.Models.Entities;

namespace weqi_store_api.Data
{
    public class WeqiDbContext : IdentityDbContext
    {
        public virtual DbSet<RefreshTokens> RefreshTokens { get; set; }
        public  DbSet<Product> Products { get; set; }
        public  DbSet<ProductImage> ProductImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); 
            modelBuilder.Entity<ProductImage>()
               .HasKey(p => new { p.imageId});

        }



        public WeqiDbContext(DbContextOptions<WeqiDbContext> options) : base(options) {

            
        }
    }
}
