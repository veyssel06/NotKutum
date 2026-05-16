using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KisiselNotYonetimSistemi.Entity.Concrete
{
    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public bool UserStatus { get; set; }
        public ICollection<Note> Notes { get; set; }

    }
}
