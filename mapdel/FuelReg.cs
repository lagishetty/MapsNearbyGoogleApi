using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mapdel
{
    class FuelReg
    {
        [AutoIncrement, PrimaryKey]
        public int FuelId { get; set; }

        public string CompanyName { get; set; }
        //public string CompanyAddress { get; set; }

        [ForeignKey(typeof(Login))]     // Specify the foreign key
        public int LoginId { get; set; }

        public double LatValue { get; set; }
        public double LongValue { get; set; }

        public string myval { get; set; }

        
    }
}
