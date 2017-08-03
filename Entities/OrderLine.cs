using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NKStrata.Models
{
    public class OrderLine
    {
        public int OrderID { get; set; }
        public Product Product { get; set; }
    }
}