using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planiranje.Models
{
    public class Mjesecni_plan
    {		
		[Key]
        public int ID_plan { get; set; }        
        public int ID_pedagog { get; set; }
        [DisplayName("Školska godina")]
        [Required(ErrorMessage ="Obavezno polje")]
        [Range(1,int.MaxValue, ErrorMessage ="Obavezno polje")]
		public int Ak_godina { get; set; }        
		[DisplayName("Naziv plana")]
        [Required(ErrorMessage ="Obavezno polje")]
		public string Naziv { get; set; }		
		public string Opis { get; set; }
    }
}