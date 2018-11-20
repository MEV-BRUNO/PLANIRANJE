using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planiranje.Models
{
    public class OS_Plan_2_podrucje
    {
        [Key]
        [Required]
        public int Id_plan { get; set; }
        [Required]
        public int Red_br_podrucje { get; set; }
        [Required(ErrorMessage ="Ovo je obavezno polje")]
        [DisplayName("Posao")]
        public string Opis_podrucje { get; set; }
        [Required(ErrorMessage = "Ovo je obavezno polje")]
        [DisplayName("Ciljevi")]
        public string Cilj { get; set; }
        [Required(ErrorMessage = "Ovo je obavezno polje")]
        [DisplayName("Zadaci")]
        public string Zadaci { get; set; }
        [Required(ErrorMessage = "Ovo je obavezno polje")]
        public string Subjekti { get; set; }
        [Required(ErrorMessage = "Ovo je obavezno polje")]
        [DisplayName("Oblik/metoda rada")]
        public string Oblici { get; set; }
        [Required(ErrorMessage = "Ovo je obavezno polje")]        
        [DisplayName("Vrijeme realizacije")]
        public string Vrijeme { get; set; }
        [Required]        
        public int Id_glavni_plan { get; set; }
        [Required(ErrorMessage = "Ovo je obavezno polje")]
        public int Sati { get; set; }
    }
}
