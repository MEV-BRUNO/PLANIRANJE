using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Planiranje.Models
{
	public class MjesecniModel
	{
		public List<SelectListItem> GodisnjiPlanovi { get; set; }
		public List<Mjesecni_plan> MjesecniPlanovi { get; set; }
		public List<Mjesecni_detalji> MjesecniDetalji { get; set; }
		public Mjesecni_plan MjesecniPlan { get; set; }
		public List<SelectListItem> Aktivnosti { get; set; }
		public List<SelectListItem> PodrucjaRada { get; set; }
		public List<SelectListItem> Subjekti { get; set; }
		public Mjesecni_detalji mjesecniDetalj { get; set; }
}
}