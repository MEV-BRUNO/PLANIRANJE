using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planiranje.Models
{
    public class Godisnji_plan
    {
		public int Redni_broj { get; set; }
		[Required]
        public int Id_god { get; set; }
        [Required(ErrorMessage ="Obavezno polje!")]
        public string Naziv { get; set; }
		[Required]
		public int Id_pedagog { get; set; }
		[Required]
		[DisplayName("Akademska godina")]
		public string Ak_godina { get; set; }
        [Required]
		[DisplayName("Broj radnih dana")]
		public int Br_radnih_dana { get; set; }
        [Required]
		[DisplayName("Broj dana GO")]
		public int Br_dana_godina_odmor { get; set; }
        [Required]
		[DisplayName("Ukupno radnih dana")]
		public int Ukupni_rad_dana { get; set; }
        [Required]
		[DisplayName("Godisnji fond sati")]
		public int God_fond_sati { get; set; }
    }
}