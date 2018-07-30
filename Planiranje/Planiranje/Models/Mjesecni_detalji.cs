using System;
using System.Collections.Generic;
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
		public string Podrucje { get; set; }
		[Required(ErrorMessage = "Obavezno polje.")]
		public string Aktivnost { get; set; }
		[Required(ErrorMessage = "Obavezno polje.")]
		public string Suradnici { get; set; }
		[Required(ErrorMessage = "Obavezno polje.")]
		[DataType(DataType.DateTime)]
        public DateTime Vrijeme { get; set; }
		[Required(ErrorMessage = "Obavezno polje.")]
		public int Br_sati { get; set; }
		[Required(ErrorMessage = "Obavezno polje.")]
		public string Biljeska { get; set; }
    }
}