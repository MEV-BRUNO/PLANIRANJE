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
    }
}