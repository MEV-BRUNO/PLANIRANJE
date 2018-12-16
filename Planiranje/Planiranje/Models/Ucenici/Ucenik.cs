using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planiranje.Models.Ucenici
{
    public class Ucenik
    {
        [Key]
        public int Id_ucenik { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        [DisplayName("Ime i prezime")]
        public string ImePrezime { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        public int Spol { get; set; }
        [Required(ErrorMessage ="Obavezno polje")]
        [StringLength(11,MinimumLength =11,ErrorMessage ="OIB nije valjan")]
        public string Oib { get; set; }
        [Required(ErrorMessage ="Obavezno polje")]
        public string Grad { get; set; }
        [Required(ErrorMessage ="Obavezno polje")]
        public string Adresa { get; set; }        
        [DisplayName("Bilješka")]
        public string Biljeska { get; set; }
        [Required(ErrorMessage ="Obavezno polje")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode =true)]
        [DisplayName("Datum rođenja")]
        public DateTime Datum { get; set; }
        /// <summary>
        /// id razreda je ovdje bitan samo kod dodavanja novog učenika, kasnije to nema veze
        /// </summary>
        public int Id_razred { get; set; }
    }
}