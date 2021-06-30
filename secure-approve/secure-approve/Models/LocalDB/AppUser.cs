using System;
using System.Collections.Generic;

#nullable disable

namespace secure_approve.Models.localdb
{
    public partial class AppUser
    {
        public int Userid { get; set; }
        public string Username { get; set; }
        public string Userpass { get; set; }
        public string Useremail { get; set; }
        public int UserRoleId { get; set; }

        public virtual AppRole UserRole { get; set; }
    }
}
