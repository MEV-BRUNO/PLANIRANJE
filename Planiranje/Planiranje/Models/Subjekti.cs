using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planiranje.Models
{
    public class Subjekti
	{
		public int Red_br { get; set; }
		[Required(ErrorMessage = "Obavezno polje.")]
		[DisplayName("Id")]
		public int ID_subjekt { get; set; }
		[Required(ErrorMessage = "Obavezno polje.")]
		[DisplayName("Naziv")]
		public string Naziv { get; set; }

        
    }
}