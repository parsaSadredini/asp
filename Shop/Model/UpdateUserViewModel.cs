using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Model
{
    public class UpdateUserViewModel 
    {
        [Required]
        [StringLength(100)]
        public string UserName { get; set; }
        //[StringLength(100)]
        public string FullName { get; set; }



        public ICollection<UserRoles> UserRoles { get; set; }


        
    }
}
