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
        public int Id_plan { get; set; }       
        public int Id_aktivnost { get; set; }        
        public int Red_br_akcija { get; set; }
        [Required(ErrorMessage ="Obavezno polje")]
        [DisplayName("Naziv aktivnosti")]
        public string Opis_akcija { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        [Range(0,Int32.MaxValue,ErrorMessage ="Vrijednost mora biti veća ili jednaka 0")]
        public int Sati { get; set; }
    }
}