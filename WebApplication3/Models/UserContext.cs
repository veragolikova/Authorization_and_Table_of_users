using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication3.Models
{
    public class UserContext : IdentityDbContext<User>
    {
        public UserContext() : base("UsersDb")//"DefaultConnection"
        { }
    }
}