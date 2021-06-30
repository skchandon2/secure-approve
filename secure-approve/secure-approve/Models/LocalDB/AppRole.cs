using System;
using System.Collections.Generic;

#nullable disable

namespace secure_approve.Models.localdb
{
    public partial class AppRole
    {
        public AppRole()
        {
            AppUsers = new HashSet<AppUser>();
        }

        public int Roleid { get; set; }
        public string Rolename { get; set; }

        public virtual ICollection<AppUser> AppUsers { get; set; }
    }
}
