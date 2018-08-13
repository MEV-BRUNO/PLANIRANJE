using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Planiranje.Models
{
	public class SSModel
	{
		public int ID_GODINA { get; set; }
		public int ID_PLAN { get; set; }
		public List<SS_Plan> SS_Planovi { get; set; }
		public List<SelectListItem> GodisnjiPlanovi { get; set; }
		public List<SS_Plan_podrucje> SS_Podrucja { get; set;  }
		public SS_Plan SS_Plan { get; set; }
		public SS_Plan_podrucje SS_Plan_Podrucje { get; set; }
		public string Ak_godina { get; set; }
		public List<SelectListItem> PodrucjaDjelovanja { get; set; }
		public List<SelectListItem> Zadace { get; set; }
		public List<SelectListItem> Oblici { get; set; }
		public List<SelectListItem> Suradnici { get; set; }
		public List<SelectListItem> Ciljevi { get; set; }
	}
}