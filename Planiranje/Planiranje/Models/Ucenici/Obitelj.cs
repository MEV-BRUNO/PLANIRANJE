using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planiranje.Models.Ucenici
{
    public class Obitelj
    {
        [Key]
        public int Id_obitelj { get; set; }
        public int Id_ucenik { get; set; }
        [Required(ErrorMessage ="Obavezno polje")]
        public string Ime { get; set; }
        [Required(ErrorMessage ="Obavezno polje")]
        public string Prezime { get; set; }
        [Required(ErrorMessage ="Obavezno polje")]
        public string Svojstvo { get; set; }
        public string Adresa { get; set; }
        public string Zanimanje { get; set; }
        public string Kontakt { get; set; }
        [DisplayName("Ime i prezime")]
        public string ImePrezime { get { return Ime + " " + Prezime; } }
    }
}