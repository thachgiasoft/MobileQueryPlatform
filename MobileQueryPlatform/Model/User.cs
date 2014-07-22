using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class User
    {
        public decimal ID
        {
            get;
            set;
        }

        public string UserCode
        {
            get;
            set;
        }

        public string UserName
        {
            get;
            set;
        }

        public string UPassword
        {
            get;
            set;
        }
        public bool IsAdmin
        {
            get;
            set;
        }
        public bool Enabled
        {
            get;
            set;
        }
    }
}
