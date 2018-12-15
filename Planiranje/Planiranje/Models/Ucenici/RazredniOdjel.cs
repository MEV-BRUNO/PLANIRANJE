using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planiranje.Models.Ucenici
{
    public class RazredniOdjel
    {
        [Key]
        public int Id { get; set; }
        public int Id_skola { get; set; }
        public int Sk_godina { get; set; }
        [Required(ErrorMessage ="Ovo je polje obavezno")]
        public string Naziv { get; set; }
        [Required(ErrorMessage = "Ovo je polje obavezno")]
        [Range(1,14,ErrorMessage ="Raspon vrijednosti je od 1 do 14")]
        public int Razred { get; set; }
        [Required(ErrorMessage = "Ovo je polje obavezno")]
        [DisplayName("Razrednik")]
        public int Id_razrednik { get; set; }
        public int Id_pedagog { get; set; }
    }
}