using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mapdel
{
    class UserReg
    {
        [AutoIncrement, PrimaryKey]
        public int UserID { get; set; }
        public string Name { get; set; }

        public string Gender { get; set; }
        public string State { get; set; }
       
        [ForeignKey(typeof(Login))]  // Specify the foreign key
        public int LoginId { get; set; }
    }
}
