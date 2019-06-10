using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planiranje.Models
{
    public class OS_Plan_2_aktivnost
    {
        [Key]        
        public int Id_plan { get; set; }        
        public int Id_podrucje { get; set; }        
        public int Red_br_aktivnost { get; set; }
        [Required(ErrorMessage ="Obavezno polje")]
        [DisplayName("Naziv zadatka")]
        public string Opis_aktivnost { get; set; }
        [Required(ErrorMessage ="Obavezno polje")]
        [Range(0,int.MaxValue,ErrorMessage ="Vrijednost mora biti veća ili jednaka 0")]
        public int Sati { get; set; }
    }
}