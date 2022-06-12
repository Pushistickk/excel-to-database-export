using System;
using System.Collections.Generic;

namespace excel_to_database
{
    public partial class Record
    {
        public int Id { get; set; }
        public int Accountnumber { get; set; }
        public string Fnp { get; set; } = null!;
        public int Regionid { get; set; }
        public int Cityid { get; set; }
        public string Streetadress { get; set; } = null!;
        public string Streetnumber { get; set; } = null!;
        public string Apartmentnumber { get; set; } = null!;
        public DateOnly Datestart { get; set; }
        public DateOnly Dateend { get; set; }
        public string Metertype { get; set; } = null!;
        public int Meterreading { get; set; }
    }
}
