using System;
using System.Collections.Generic;

#nullable disable

namespace CrossBorder.Models
{
    public partial class Sale
    {
        public string ProductId { get; set; }
        public string CountryId { get; set; }
        public decimal Price { get; set; }

        public virtual Country Country { get; set; }
        public virtual Product Product { get; set; }
    }
}
