using CrossBorder.Models;
using System;
using System.Collections.Generic;

#nullable disable

namespace CrossBorder.Models
{
    public partial class Classified
    {
        public string ProductId { get; set; }
        public string TypeId { get; set; }

        public virtual Product Product { get; set; }
        public virtual Type Type { get; set; }
    }
}
