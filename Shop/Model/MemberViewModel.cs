using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Model
{
    public class MemberViewModel
    {
        [Required]
        [StringLength(100)]
        public string UserName { get; set; }
        [Required]
        [StringLength(500)]
        public string Password { get; set; }
    }
}
