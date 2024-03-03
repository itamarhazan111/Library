using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Lib
{
    public class User
    {
        public string UserName { get; private set; }
        private SecureString Password { get; set; }
        public bool IsAdmin { get; private set; }
        public User(string username, SecureString password, bool _isEmployee )
        {
            UserName = username;
            Password = password;
            IsAdmin = _isEmployee;
        }

        public override int GetHashCode()
        {
            return new NetworkCredential("", Password).Password.GetHashCode();
        }

    }
}
