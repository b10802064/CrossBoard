using System;
using System.Collections.Generic;

#nullable disable

namespace CrossBorder.Models
{
    public partial class Product
    {
        public Product()
        {
            Classifieds = new HashSet<Classified>();
            Sales = new HashSet<Sale>();
            Shoppinglists = new HashSet<Shoppinglist>();
        }

        public string ProductId { get; set; }
        public string ProductJP { get; set; }
        public string ProductCN { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string Photo { get; set; }
        public string Photo2 { get; set; }


        public virtual ICollection<Classified> Classifieds { get; set; }
        public virtual ICollection<Sale> Sales { get; set; }
        public virtual ICollection<Shoppinglist> Shoppinglists { get; set; }
    }
}
