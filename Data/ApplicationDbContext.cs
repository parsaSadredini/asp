using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Common.Utilities;
using System.Reflection;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Data
{
    public class ApplicationDbContext : DbContext
        //DbContext
    {
        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {

        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Data Source=. ; Initial Catalog = ShopDb ; Integerated Security=true");
        //    base.OnConfiguring(optionsBuilder);

        //}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
             
            var entitiesAssembly = typeof(IEntity).Assembly;
            // introducing all Entities
            modelBuilder.RegisterAllEntities<IEntity>(entitiesAssembly);
            // introducing all Entities Configuration(Fluent API)
            modelBuilder.RegisterEntityTypeConfiguration(entitiesAssembly);
            // to prevent our parent records from being deleted when thier children exist
            modelBuilder.AddRestrictDeleteBehaviorConvention();
            // better performance for GUID 
            modelBuilder.Entity<Product>().Property(x=>x.ID).HasAnnotation("default", "NEWSEQUENTIALID()");
            modelBuilder.Entity<UserRoles>().HasOne(x => x.Role).WithMany(x => x.UserRoles).HasForeignKey(x => x.RoleId);
            modelBuilder.Entity<UserRoles>().HasOne(x => x.User).WithMany(x => x.UserRoles).HasForeignKey(x => x.UserId);
            //modelBuilder.Entity<User>().HasMany(x => x.UserRoles).WithOne(x => x.User).HasForeignKey(x => x.UserId);
            //modelBuilder.Entity<Role>().HasMany(x => x.UserRoles).WithOne(x => x.Role).HasForeignKey(x => x.RoleId);
        }
        //it could be another alternatives
        //public DbSet<User> Users { get; set; }
        //public DbSet<Rule> Users { get; set; }
        //public DbSet<Category> Users { get; set; }
        //public DbSet<Product> Users { get; set; }

        public override int SaveChanges()
        {
            _cleanString();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            _cleanString();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            _cleanString();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            _cleanString();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void _cleanString()
        {
            var changedEntities = ChangeTracker.Entries()
                .Where(x => x.State == EntityState.Added || x.State == EntityState.Modified);
            foreach (var item in changedEntities)
            {
                if (item.Entity == null)
                    continue;

                var properties = item.Entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.CanRead && p.CanWrite && p.PropertyType == typeof(string));

                foreach (var property in properties)
                {
                    var propName = property.Name;
                    var val = (string)property.GetValue(item.Entity, null);

                    if (val.HasValue())
                    {
                        var newVal = val.Fa2En().FixPersianChars();
                        if (newVal == val)
                            continue;
                        property.SetValue(item.Entity, newVal, null);
                    }
                }
            }
        }
    }
}
