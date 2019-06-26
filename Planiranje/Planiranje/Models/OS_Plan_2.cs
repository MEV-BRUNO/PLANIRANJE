using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planiranje.Models
{
    public class OS_Plan_2
    {
        [Key]        
        public int Id_plan { get; set; }        
        public int Id_pedagog { get; set; }
        [Required(ErrorMessage ="Obavezno polje")]
		[DisplayName("Šk. godina")]
		public int Ak_godina { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
		[DisplayName("Naziv plana")]
		public string Naziv { get; set; }  
        [Required(ErrorMessage ="Obavezno polje")]
		[DisplayName("Opis plana")]
		public string Opis { get; set; }
    }
}