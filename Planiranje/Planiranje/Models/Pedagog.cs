using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Planiranje.Models
{
    public class Pedagog
    {
        public int Id;
        public string ime;
        public string prezime;
        public string email;
        public int lozinka;
        public DateTime licenca; //datum trajanja pristupa
        public int id_skola;
        public bool aktivan;
        public string titula;
    }
}