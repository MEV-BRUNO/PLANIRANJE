using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Planiranje.Models
{
	public class PlaniranjeModel
	{
		public SelectList PopisSkola { get; set; }
		public Pedagog Pedagog { get; set; }
        [Range(1,int.MaxValue,ErrorMessage ="Odaberite školu")]
		public int SelectedSchool { get; set; }
	}
}