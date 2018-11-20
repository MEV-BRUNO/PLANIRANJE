using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planiranje.Models
{
    public class OS_Plan_1
    {
        [Key]
        [Required]
        public int Id_plan { get; set; }
        [Required]
        public int Id_pedagog { get; set; }
        [Required(ErrorMessage ="Školska godina je obavezna")]
		[DisplayName("Šk. godina")]        
		public int Ak_godina { get; set; }
        [Required(ErrorMessage = "Naziv plana je obavezan")]
		[DisplayName("Naziv plana")]
		public string Naziv { get; set; }
        [Required(ErrorMessage = "Opis plana je obavezan")]
		[DisplayName("Opis plana")]
		public string Opis { get; set; }
    }
}