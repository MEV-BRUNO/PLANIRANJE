using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Planiranje.Models
{
    public class Dnevnik_detalji
    {
        public int ID_dnevnik { get; set; }
        public int Subjekt { get; set; }
        public DateTime Vrijeme_od { get; set; }
        public DateTime Vrijeme_do { get; set; }
        public int Aktivnost { get; set; }
        public string Suradnja { get; set; }
        public string Zakljucak { get; set; }

    }
}