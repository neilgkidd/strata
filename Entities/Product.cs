using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace NKStrata.Models
{
    public class Product
    {
        public string ProductCode { get; set; }
        public decimal UnitPrice { get; set; }
        public string Description { get; set; }
        [Range(0,999)]
        public int Quantity { get; set; }
    }
}