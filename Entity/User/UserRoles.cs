using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class UserRoles : BaseEntity
    {
        
        public int? UserId { get; set; }
        public int? RoleId { get; set; }
        public Role Role { get; set; }
        public User User { get; set; }
    }
}
