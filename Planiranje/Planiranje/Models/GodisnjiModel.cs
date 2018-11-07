using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Planiranje.Models
{
    public class GodisnjiModel
    {
        public List<Godisnji_detalji> GodisnjiDetalji { get; set; }
        public Godisnji_plan GodisnjiPlan { get; set; }
        public List<Sk_godina> SkolskaGodina { get; set; }
        public IEnumerable<SelectListItem> SkolskaGodinaItems
        {
            get { return new SelectList(SkolskaGodina, "Sk_Godina", "Sk_Godina"); }
        }
    }
}