using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Planiranje.Models
{
    public class SS_Plan_podrucje
    {
        public int ID_plan { get; set; }
        public int Red_br_podrucje { get; set; }
        public int Opis_podrucje { get; set; }
        public string Svrha { get; set; }
        public string Zadaca { get; set; }
        public string Sadrzaj { get; set; }
        public string Oblici { get; set; }
        public string Suradnici { get; set; }
        public string Mjesto { get; set; }
        public DateTime Vrijeme { get; set; }
        public string Ishodi { get; set; }
        public double Sati { get; set; }
    }
}