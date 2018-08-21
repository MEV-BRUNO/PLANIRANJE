using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planiranje.Models
{
	public class Akt_Akc
	{
		public int Red_br { get; set; }
		public int Id_akcija { get; set; }
		[Required(ErrorMessage = "Obavezno polje")]
		[DisplayName("Akcija")]
		public string Naziv_Akcija { get; set; }
		[Required(ErrorMessage = "Obavezno polje")]
		[DisplayName("Aktivnost")]
		public string Naziv_Aktivnost { get; set; }
	}
}