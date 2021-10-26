using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities
{
    public class Member : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string UserName { get; set; }
        [Required]
        [StringLength(500)]
        public string PasswordHash { get; set; }
    }
}
