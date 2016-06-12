using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfApplication2.Model.Vo
{
    public class User
    {
        private string id;
        private string pwd;
        private string power;
        List<string> privileges;


        public User()
        {
        }
        public User(string id, string pwd)
        {
            this.id = id;
            this.pwd = pwd;
        }
        public User(string id, string pwd, string power = "normal", List<string> powerOfBuildingId = null)
        {
            this.id = id;
            this.pwd = pwd;
            this.power = power;
            this.privileges = powerOfBuildingId;
        }

        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Password
        {
            get { return pwd; }
            set { pwd = value; }
        }

        public string Power
        {
            get { return power; }
            set { power = value; }
        }

        public bool IsAdmin
        {
            get { return power.Equals("admin",StringComparison.CurrentCultureIgnoreCase); }
        }

        public bool IsAdministrator()
        {
            return this.id.Equals("admin",StringComparison.CurrentCultureIgnoreCase); 
        }
        public List<string> Privileges
        {
            get { return privileges;  }
            set { privileges = value; }
        }
        
        /// <summary>
        ///判断非空和是否可以入库
        /// </summary>
        /// <returns></returns>
        public bool CanBeInsertDB()
        {
            if(!id.Equals("")&&!Password.Equals("")&&!privileges.Equals(""))
            {
                return true;
            }
            return false;
        }
    }
}
