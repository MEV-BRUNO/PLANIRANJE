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
		public SS_Plan SS_Plan { get; set; }
	}
}