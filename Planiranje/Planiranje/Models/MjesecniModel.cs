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
		public Mjesecni_plan MjesecniPlan { get; set; }
	}
}