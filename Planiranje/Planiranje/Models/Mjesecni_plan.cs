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
		[Required]
        [Key]
        public int ID_plan { get; set; }
        [Required]
        public int ID_pedagog { get; set; }
        [DisplayName("Školska godina")]
		public int Ak_godina { get; set; }        
		[DisplayName("Naziv plana")]
		public string Naziv { get; set; }		
		public string Opis { get; set; }
    }
}