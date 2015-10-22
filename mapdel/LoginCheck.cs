using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mapdel
{
    class LoginCheck
    {
        [AutoIncrement, PrimaryKey]
        public int LoginId { get; set; }
        public string LoginName { get; set; }
        public string Password { get; set; }
        public bool IsCompany { get; set; }
    }
}
