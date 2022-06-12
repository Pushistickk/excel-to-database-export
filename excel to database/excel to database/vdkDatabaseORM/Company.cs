using System;
using System.Collections.Generic;

namespace excel_to_database
{
    public partial class Company
    {
        public Company()
        {
            Layouts = new HashSet<Layout>();
        }

        public int Id { get; set; }
        public string Companyname { get; set; } = null!;

        public virtual ICollection<Layout> Layouts { get; set; }
    }
}
