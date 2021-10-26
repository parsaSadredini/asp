using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class Product:BaseEntity<Guid>
    {
        public string name { get; set; }
        public string imageUrl { get; set; }
        public double price { get; set; }
        public int? OperatorID { get; set; }
        public User Operator { get; set; }

        public int CategoryID { get; set; }
        public Category Category { get; set; } 
    }

    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(x => x.name).IsRequired(true).HasMaxLength(200);
            builder.Property(x => x.imageUrl).IsRequired(true);
            builder.Property(x => x.price).IsRequired(true);
            builder.HasOne(x => x.Category).WithMany(p => p.Products).HasForeignKey(f => f.CategoryID);
            builder.HasOne(x => x.Operator).WithMany(p => p.Products).HasForeignKey(f => f.OperatorID);
        }
    }
}
