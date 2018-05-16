using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planiranje.Models
{
    public class Skola
    {
        [Required]
        public int Id_skola { get; set; }
        [Required]
        public string Naziv { get; set; }
        [Required]
        public string Adresa { get; set; }
        [Required]
        public string Grad { get; set; }
        [Required]
        public string Tel { get; set; }
        [Required]
        [DataType(DataType.Url)]
        public string URL { get; set; }
        [Required]
        public string Kontakt { get; set; }
    }
}