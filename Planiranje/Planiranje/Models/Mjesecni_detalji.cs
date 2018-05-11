using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Planiranje.Models
{
    public class Mjesecni_detalji
    {
        public int ID_plan { get; set; }
        public int Red_br { get; set; }
        public int Podrucje { get; set; }
        public int Aktivnost { get; set; }
        public string Suradnici { get; set; }
        public DateTime Vrijeme { get; set; }
        public double Br_sati { get; set; }
        public string Biljeska { get; set; }
    }
}