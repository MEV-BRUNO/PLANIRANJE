using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Planiranje.Models.Ucenici
{
    public class PracenjeUcenikaModel
    {
        public Ucenik Ucenik { get; set; }
        public List<Sk_godina> SkGodine { get; set; }
        public List<RazredniOdjel> RazredniOdjeli { get; set; }
        public RazredniOdjel Razred { get; set; }
        public Nastavnik Razrednik { get; set; }
        public List<Obitelj> ListaObitelji { get; set; }
        public Obitelj Obitelj { get; set; }
        public SelectList Odaberi { get; set; }
        public Pracenje_ucenika PracenjeUcenika { get; set; }
    }
}