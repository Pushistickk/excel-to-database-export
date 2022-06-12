using System;
using System.Collections.Generic;

namespace excel_to_database
{
    public partial class Layout
    {
        public int Accountnumber { get; set; }
        public int Companyid { get; set; }
        public string Layout1 { get; set; } = null!;
        public string Scheme { get; set; } = null!;

        public virtual Company Company { get; set; } = null!;
    }
}
