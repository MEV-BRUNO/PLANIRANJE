using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planiranje.Models.Ucenici
{
    public class Neposredni_rad
    {
        [Key]
        public int Id_rad { get; set; }
        public int Id_ucenik { get; set; }
        [Required(ErrorMessage ="Obavezno polje")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Datum { get; set; }
        [Required(ErrorMessage ="Obavezno polje")]
        public string Napomena { get; set; }
    }
}