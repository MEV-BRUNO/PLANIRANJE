using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Planiranje.Models
{
    public class Pedagog
    {
        public int Id { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Email { get; set; }
        public string Lozinka { get; set; }
        public DateTime Licenca { get; set; } //datum trajanja pristupa
        public int Id_skola { get; set; }
        public bool Aktivan { get; set; }
        public string Titula { get; set; }
    }
}