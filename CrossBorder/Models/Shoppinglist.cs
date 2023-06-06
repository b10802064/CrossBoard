using System;
using System.Collections.Generic;

#nullable disable

namespace CrossBorder.Models
{
    public partial class Shoppinglist
    {
        public string CustomerId { get; set; }
        public string ProductId { get; set; }
        public int Amount { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Product Product { get; set; }
    }
}
