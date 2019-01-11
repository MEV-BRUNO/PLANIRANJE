using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planiranje.Models.Ucenici
{
    public class Mjesecna_biljeska
    {
        [Key]
        public int Id { get; set; }
        public int Id_ucenik_biljeska { get; set; }
        [Required(ErrorMessage ="Obavezno polje")]
        public string Mjesec { get; set; }
        [Required(ErrorMessage ="Obavezno polje")]
        [DisplayName("Bilješka")]
        public string Biljeska { get; set; }
        [Required(ErrorMessage ="Obavezno polje")]
        [Range(1,Int32.MaxValue,ErrorMessage ="Obavezno polje")]
        public int Sk_godina { get; set; }
    }
}