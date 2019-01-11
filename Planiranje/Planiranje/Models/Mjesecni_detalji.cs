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
        [Key]
		[Required]
		public int ID { get; set; }
		[Required]        
		public int ID_plan { get; set; }	
        [Required(ErrorMessage ="Područje rada je obavezno")]
		[DisplayName("Područje rada")]
		public string Podrucje { get; set; }
        [Required(ErrorMessage = "Aktivnost je obavezna")]
        [DisplayName("Aktivnost")]
		public string Aktivnost { get; set; }
        [Required(ErrorMessage = "Suradnici su obavezni")]
        [DisplayName("Suradnici")]
		public string Suradnici { get; set; }		
		//[DataType(DataType.Date)]
		[DisplayName("Datum izvršenja")]
        [Required(ErrorMessage = "Datum je obavezan")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
		public DateTime Vrijeme { get; set; }
        [Required(ErrorMessage = "Vrijeme je obavezno")]
        [DisplayName("Vrijeme rada")]
        [Range(0,Int32.MaxValue,ErrorMessage ="Vrijednost mora biti veća ili jednaka 0")]
		public int Br_sati { get; set; }		
		[DisplayName("Bilješke")]
        [Required(ErrorMessage = "Bilješka je obavezna")]
        public string Biljeska { get; set; }
        [DisplayName("Subjekti")]
        [Required(ErrorMessage = "Subjekti su obavezni")]
        public string Subjekti { get; set; }
    }
}