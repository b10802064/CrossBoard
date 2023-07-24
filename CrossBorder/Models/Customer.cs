using System;
using System.Collections.Generic;

#nullable disable

namespace CrossBorder.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Shoppinglists = new HashSet<Shoppinglist>();
        }

        public string CustomerId { get; set; }
        public string CusdtomerName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? lineid { get; set; }

        public virtual ICollection<Shoppinglist> Shoppinglists { get; set; }
    }
}
