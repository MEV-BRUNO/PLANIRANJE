using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Planiranje.Models
{
	public class AkcijeModel
	{
		public List<Akt_Akc> akcije { get; set; }
		public Aktivnost_akcija akcija { get; set; }
		public List<Aktivnost> aktivnosti { get; set; }
	}
}