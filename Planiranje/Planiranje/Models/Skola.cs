using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planiranje.Models
{
    public class Skola
    {
        [Key]
        [Required]
        public int Id_skola { get; set; }        
        public string Naziv { get; set; }        
        public string Adresa { get; set; }        
        public string Grad { get; set; }
        [Required(ErrorMessage ="Obavezno polje")]
        public string Tel { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        [DataType(DataType.Url)]
        public string URL { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        public string Kontakt { get; set; }
        public int Vrsta { get; set; }
        [DisplayName("Vrsta")]
        public string Tip { get { if (Vrsta == 0) return "Osnovna škola"; else return "Srednja škola"; } }
    }
}