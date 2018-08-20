using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planiranje.Models
{
    public class Mjesecni_detalji
	{
		[Required]
		public int ID { get; set; }
		[Required]
		public int ID_plan { get; set; }
		[Required(ErrorMessage = "Obavezno polje.")]
		public int Red_br { get; set; }
		[Required(ErrorMessage = "Obavezno polje.")]
		[DisplayName("Područje rada")]
		public string Podrucje { get; set; }
		[Required(ErrorMessage = "Obavezno polje.")]
		[DisplayName("Aktivnost")]
		public string Aktivnost { get; set; }
		[Required(ErrorMessage = "Obavezno polje.")]
		[DisplayName("Suradnici")]
		public string Suradnici { get; set; }
		[Required(ErrorMessage = "Datum mora biti veći od trenutnog.")]
		[DataType(DataType.Date)]
		[DisplayName("Vrijeme izvršenja")]
		[DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
		public DateTime Vrijeme { get; set; }
		[Required(ErrorMessage = "Obavezno polje.")]
		[DisplayName("Vrijeme rada")]
		[Range(1, 999, ErrorMessage = "Vrijeme rada može biti iymeđu 1 i 999 sati")]
		public int Br_sati { get; set; }
		[Required(ErrorMessage = "Obavezno polje.")]
		[DisplayName("Bilješke")]
		public string Biljeska { get; set; }
    }
}