using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace freelance.api.Models
{
    public class User : IdentityUser<int>
    {
        public ICollection<UserRole> UserRoles { get; set; }
        public DateTime LastActive { get; set; }
    }
}