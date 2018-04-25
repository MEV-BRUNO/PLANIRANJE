using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Planiranje.Models
{
    public class Godisnji_detalji
    {
        public int id_god;
        public int mjesec;
        public string naziv_mjesec;
        public int ukupno_dana;
        public int radnih_dana;
        public int subota_dana;
        public int blagdana_dana;
        public int nastavnih_dana;
        public int praznika_dana;
        public int br_sati;
        public int odmor_dana;
        public int odmor_sati;
        public int mj_fond_sati;
        public int br_rad_dana_sk_god;
        public int br_dana_god_odmor;
        public int ukupno_rad_dana;
        public int god_fond_sati;
    }
}