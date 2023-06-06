using System;
using System.Collections.Generic;

#nullable disable

namespace CrossBorder.Models
{
    public partial class Type
    {
        public Type()
        {
            Classifieds = new HashSet<Classified>();
        }

        public string TypeId { get; set; }
        public string TypeName { get; set; }

        public virtual ICollection<Classified> Classifieds { get; set; }
    }
}
