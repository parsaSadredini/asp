using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities
{
    public class User : BaseEntity
    {
        public User()
        {
            IsActive = true;
            LastDateLogin = DateTime.Now;
        }
        [Required]
        [StringLength(100)]
        public string UserName { get; set; }
        [Required]
        [StringLength(500)]
        public string PasswordHash { get; set; }
        //[StringLength(100)]
        public string Fullname { get; set; }
        public int Age { get; set; }
        public GenderType Gender { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<UserRoles> UserRoles { get; set; }
        public DateTimeOffset LastDateLogin { set; get; }

        public ICollection<Product> Products { get; set; }
        


    }



    public enum GenderType
    {
        [Display(Name = "مرد")]
        Man = 1,
        [Display(Name = "زن")]
        Woman = 2
    }
}
