using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planiranje.Models.Ucenici
{
    public class Promatranje_ucenika
    {
        [Key]
        public int Id { get; set; }
        public int Id_ucenik_razred { get; set; }        
        public int Id_pedagog { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage ="Obavezno polje")]
        public DateTime Nadnevak { get; set; }
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        [Required(ErrorMessage ="Obavezno polje")]
        public DateTime Vrijeme { get; set; }
        [Required(ErrorMessage ="Obavezno polje")]
        [DisplayName("Socioekonomski status učenika")]
        public string SocStatusUcenika { get; set; }
        [Required(ErrorMessage ="Obavezno polje")]
        [DisplayName("Cilj promatranja")]
        public string Cilj { get; set; }
        //[Required(ErrorMessage ="Obavezno polje")]
        public string Spremnost { get; set; }
        //[Required(ErrorMessage = "Obavezno polje")]
        public string Prilagodljivost { get; set; }
        //[Required(ErrorMessage = "Obavezno polje")]
        public string Odnos { get; set; }
        //[Required(ErrorMessage = "Obavezno polje")]
        public string Doprinos { get; set; }
        //[Required(ErrorMessage = "Obavezno polje")]
        public string Opis { get; set; }
        //[Required(ErrorMessage = "Obavezno polje")]
        public string Zakljucak { get; set; }
    }
}