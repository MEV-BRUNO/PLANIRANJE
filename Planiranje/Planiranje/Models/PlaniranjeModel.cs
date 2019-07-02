using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Planiranje.Models
{
	public class PlaniranjeModel
	{
		public SelectList PopisSkola { get; set; }
		public Pedagog Pedagog { get; set; }
		public int SelectedSchool { get; set; }
	}
}