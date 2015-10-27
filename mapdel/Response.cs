using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mapdel
{
    class Response
    {
      //  public int CId { get; set; }
        public string res { get; set; }

     //   [AutoIncrement,PrimaryKey]
        public int UserResponseID { get; set; }
    }
}
