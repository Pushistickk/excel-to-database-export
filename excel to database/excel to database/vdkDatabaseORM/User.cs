using System;
using System.Collections.Generic;

namespace excel_to_database
{
    public partial class User
    {
        public int Accountnumber { get; set; }
        public string Fnp { get; set; } = null!;
        public string Region { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Streetadress { get; set; } = null!;
        public string Streetnumber { get; set; } = null!;
        public string Apartmentnumber { get; set; } = null!;
    }
}
