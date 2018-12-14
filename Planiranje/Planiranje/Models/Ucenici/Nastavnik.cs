using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planiranje.Models.Ucenici
{
    public class Nastavnik
    {
        [Key]
        public int Id { get; set; }
        public int Id_skola { get; set; }
        public int Id_pedagog { get; set; }
        [Required(ErrorMessage ="Obavezno polje")]        
        public string Ime { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]        
        public string Prezime { get; set; }
        public string Titula { get; set; }
        public string Kontakt { get; set; }
    }
}