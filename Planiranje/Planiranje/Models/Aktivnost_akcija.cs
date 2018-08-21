using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planiranje.Models
{
    public class Aktivnost_akcija
	{
		[Required(ErrorMessage = "Obavezno polje")]
		[DisplayName("Id")]
		public int Id_akcija { get; set; }
		[Required(ErrorMessage = "Obavezno polje")]
		[DisplayName("Akcija")]
		public string Naziv { get; set; }
		[Required(ErrorMessage = "Obavezno polje")]
		[DisplayName("Aktivnost")]
		public int Id_aktivnost { get; set; }
    }
}