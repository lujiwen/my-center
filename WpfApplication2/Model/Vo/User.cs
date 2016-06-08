using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfApplication2.Model.Vo
{
    public class User
    {
        string username;
        string password;
        List<string> privileges;//添加该用户所能操作的工号的名称

        public User()
        {
 
        }

        public User(string name, string pw)
        {
            username = name;
            password = pw;
        }
        public User(string name, string pw, List<string> privilege)
        {
            username = name;
            password = pw;
            privileges = privilege;
        }

        public string UserName { get { return username; } set { username = value; } }
        public string Passord { get { return password; } set { password = value; } }
        public List<string> Privileges { get { return privileges; } set { privileges = value; } }
    }
}
