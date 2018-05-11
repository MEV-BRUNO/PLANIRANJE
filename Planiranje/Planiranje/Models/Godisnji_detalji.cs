using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Planiranje.Models
{
    public class Godisnji_detalji
    {
        public int Id_god { get; set; }
        public int Mjesec { get; set; }
        public string Naziv_mjesec { get; set; }
        public int Ukupno_dana { get; set; }
        public int Radnih_dana { get; set; }
        public int Subota_dana { get; set; }
        public int Blagdana_dana { get; set; }
        public int Nastavnih_dana { get; set; }
        public int Praznika_dana { get; set; }
        public int Br_sati { get; set; }
        public int Odmor_dana { get; set; }
        public int Odmor_sati { get; set; }
        public int Mj_fond_sati { get; set; }
        public int Br_rad_dana_sk_god { get; set; }
        public int Br_dana_god_odmor { get; set; }
        public int Ukupno_rad_dana { get; set; }
        public int God_fond_sati { get; set; }
    }
}