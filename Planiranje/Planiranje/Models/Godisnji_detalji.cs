using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planiranje.Models
{
    public class Godisnji_detalji
	{
		[Required]
		public int Id { get; set; }
		[Required]
		public int Ak_god { get; set; }
		[Required]
		[DisplayName("Mjesec")]
		public int Mjesec { get; set; }
        [Required]
		[DisplayName("Naziv mjeseca")]
		public string Naziv_mjeseca { get; set; }
        [Required]
		[DisplayName("Ukupno dana")]
		public int Ukupno_dana { get; set; }
        [Required]
		[DisplayName("Radnih dana")]
		public int Radnih_dana { get; set; }
		[Required]
		[DisplayName("Subota")]
		public int Subota_dana { get; set; }
		[Required]
		[DisplayName("Nedjelja")]
		public int Nedjelja_dana { get; set; }
		[Required]
		[DisplayName("Blagdana")]
		public int Blagdana_dana { get; set; }
        [Required]
		[DisplayName("Nastavnih dana")]
		public int Nastavnih_dana { get; set; }
        [Required]
		[DisplayName("Praznika")]
		public int Praznika_dana { get; set; }
        [Required]
		[DisplayName("Broj sati")]
		public int Br_sati { get; set; }
        [Required]
		[DisplayName("Odmor dana")]
		public int Odmor_dana { get; set; }
        [Required]
		[DisplayName("Odmor sati")]
		public int Odmor_sati { get; set; }
        [Required]
		[DisplayName("Fond sati")]
		public int Mj_fond_sati { get; set; }
    }
}
