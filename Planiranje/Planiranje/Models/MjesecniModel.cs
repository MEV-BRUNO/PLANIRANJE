using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Planiranje.Models
{
	public class MjesecniModel
	{
		public List<Sk_godina> SkolskaGodina { get; set; }
		public List<Mjesecni_plan> MjesecniPlanovi { get; set; }
		public List<Mjesecni_detalji> MjesecniDetalji { get; set; }
		public Mjesecni_plan MjesecniPlan { get; set; }
		public List<Aktivnost> Aktivnosti { get; set; }
		public List<Podrucje_rada> PodrucjaRada { get; set; }
		public List<Subjekti> Subjekti { get; set; }
		public Mjesecni_detalji mjesecniDetalj { get; set; }
		public int ID_PLAN { get; set; }
		public int GODINA { get; set; }
        public IEnumerable<SelectListItem> SkGodinaItems
        {
            get { return new SelectList(SkolskaGodina, "Sk_Godina", "Sk_Godina"); }
        }
    }
}