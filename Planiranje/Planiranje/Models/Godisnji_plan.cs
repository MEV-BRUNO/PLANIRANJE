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
		[Required]
		public int Id_pedagog { get; set; }
		[DataType("Ak_godina")]
		[Display(Name = "Akademska godina")]
		[Required(ErrorMessage = "Obavezno polje!")]
		[RegularExpression("^[0-9]{4}/[0-9]{4}$", ErrorMessage = "Pogrešan format akademske godine.")]
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