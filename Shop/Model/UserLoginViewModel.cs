using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Model
{
    public class UserLoginViewModel
    {
        public string token { get; set; }
        public ICollection<UserRoles> UserRoles { get; set; }

    }
}
