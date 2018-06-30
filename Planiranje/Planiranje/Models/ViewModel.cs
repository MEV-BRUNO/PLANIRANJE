using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Planiranje.Models
{
	public class ViewModel
	{
		public List<Godisnji_detalji> GodisnjiDetalji { get; set; }
		public Godisnji_plan GodisnjiPlan { get; set; }
	}
}