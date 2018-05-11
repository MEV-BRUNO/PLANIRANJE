using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Planiranje.Models
{
    public class Skola
    {
        public int Id_skola { get; set; }
        public string Naziv { get; set; }
        public string Adresa { get; set; }
        public string Grad { get; set; }
        public int Tel { get; set; }
        public string URL { get; set; }
        public int Kontakt { get; set; }
    }
}