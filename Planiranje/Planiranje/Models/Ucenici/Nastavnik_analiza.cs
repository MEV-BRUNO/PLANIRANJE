using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planiranje.Models.Ucenici
{
    public class Nastavnik_analiza
    {
        public int Id { get; set; }
        public int Id_pedagog { get; set; }
        public int Id_skola { get; set; }
        public int Sk_godina { get; set; }
        public string Odjel { get; set; }
        public DateTime Datum { get; set; }
        public string Nastavni_sat { get; set; }
        public string Cilj_posjete { get; set; }
        public string Predmet { get; set; }
        public string Nastavna_jedinica { get; set; }
        public string Vrsta_nastavnog_sata { get; set; }
        public string Planiranje_priprema { get; set; }
        public string Izvedba_nastavnog_sata { get; set; }
        public string Vodjenje_nastavnog_sata { get; set; }
        public string Razredni_ugodjaj { get; set; }
        public string Disciplina { get; set; }
        public string Ocjenjivanje_ucenika { get; set; }
        public string Osvrt { get; set; }
        public string Prijedlozi { get; set; }
        public string Uvid { get; set; }
    }
}