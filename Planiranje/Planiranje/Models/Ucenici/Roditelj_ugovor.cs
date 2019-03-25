using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planiranje.Models.Ucenici
{
    public class Roditelj_ugovor
    {
        [Key]
        public int Id { get; set; }
        public int Id_ucenik_razred { get; set; }
        public int Id_pedagog { get; set; }
        [Required(ErrorMessage ="Obavezno polje")]
        [Range(1,int.MaxValue)]
        [DisplayName("Roditelj / skrbnik")]
        public int Id_roditelj { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        [DisplayName("Ugovor sklopljen dana")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Datum { get; set; }
        [Required(ErrorMessage ="Obavezno polje")]
        [DisplayName("Prvi cilj")]
        public string Cilj1 { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        [DisplayName("Drugi cilj")]
        public string Cilj2 { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        [DisplayName("Kako bih pomagala/pomogao popravljanju ponašanja, ja ću poduzeti sljedeće")]
        public string Poduzeto { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        [DisplayName("Predstavnik škole")]
        public string Predstavnik_skole { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        [DisplayName("Sljedeći susret")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Slijedeci_susret { get; set; }
        [Required(ErrorMessage ="Obavezno polje")]
        [DisplayName("Ostala zapažanja")]
        public string Zapazanje { get; set; }
        [DisplayName("Bilješka - 1")]
        public string Biljeska1 { get; set; }
        [DisplayName("Bilješka - 2")]
        public string Biljeska2 { get; set; }
        [DisplayName("Bilješka - 3")]
        public string Biljeska3 { get; set; }
        [DisplayName("Bilješka - 4")]
        public string Biljeska4 { get; set; }
        [DisplayName("Bilješka - 5")]
        public string Biljeska5 { get; set; }
        [DisplayName("Bilješka - 6")]
        public string Biljeska6 { get; set; }
        [Required(ErrorMessage ="Obavezno polje")]
        [DisplayName("Izvješće na kraju ugovora")]
        public string Izvjesce { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        [DisplayName("Ostala zapažanja")]
        public string Ostala_zapazanja { get; set; }
    }
}