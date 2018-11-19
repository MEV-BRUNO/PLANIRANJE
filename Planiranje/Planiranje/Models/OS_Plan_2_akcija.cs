using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planiranje.Models
{
    public class OS_Plan_2_akcija
    {
        [Key]
        [Required]
        public int Id_plan { get; set; }        
        [Required]
        public int Id_aktivnost { get; set; }
        [Required]
        public int Red_br_akcija { get; set; }
        [Required(ErrorMessage ="Ovo je obavezno polje")]
        [DisplayName("Naziv aktivnosti")]
        public string Opis_akcija { get; set; }
        [Required(ErrorMessage = "Ovo je obavezno polje")]
        [Range(0,Int32.MaxValue,ErrorMessage ="Vrijednost mora biti 0 ili veća")]
        public int Sati { get; set; }
    }
}