using System;
using System.Collections.Generic;

#nullable disable

namespace CrossBorder.Models
{
    public partial class Country
    {
        public Country()
        {
            Sales = new HashSet<Sale>();
        }

        public string CountryId { get; set; }
        public string CountryName { get; set; }
        public string Prefix { get; set; }

        public virtual ICollection<Sale> Sales { get; set; }
    }
}
