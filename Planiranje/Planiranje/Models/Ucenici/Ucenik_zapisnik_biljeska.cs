using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planiranje.Models.Ucenici
{
    public class Ucenik_zapisnik_biljeska
    {
        [Key]
        public int Id { get; set; }
        public int Id_ucenik_zapisnik { get; set; }
        [Required(ErrorMessage ="Obavezno polje")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Datum { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        [DisplayName("Sadržaj")]
        public string Sadrzaj { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        public string Dogovor { get; set; }
    }
}