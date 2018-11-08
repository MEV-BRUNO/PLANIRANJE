using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planiranje.Models
{
    public class Oblici
	{
		public int Red_br { get; set; }
		[Required(ErrorMessage = "Obavezno polje")]
		[DisplayName("Id")]
		public int Id_oblici { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
		[DisplayName("Naziv")]
		public string Naziv { get; set; }
        public int Vrsta { get; set; }
    }
}
