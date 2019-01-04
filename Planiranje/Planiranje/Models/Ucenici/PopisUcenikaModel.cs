using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Planiranje.Models.Ucenici
{
    public class PopisUcenikaModel
    {
        public Popis_ucenika Popis { get; set; }
        public List<Popis_ucenika> PopisUcenika { get; set; }
        public RazredniOdjel Razred { get; set; }
        public List<Ucenik_razred> Ucenik_razred { get; set; }
        public List<Ucenik> Ucenici { get; set; }
        public List<Obitelj> Obitelji { get; set; }
        public Ucenik_razred UcenikRazred { get; set; }
    }
}