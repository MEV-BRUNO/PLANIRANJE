using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planiranje.Models
{
    public class Dnevnik_detalji
    {
        [Required]
        public int ID_dnevnik { get; set; }
        [Required]
        public int Subjekt { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Vrijeme_od { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Vrijeme_do { get; set; }
        [Required]
        public int Aktivnost { get; set; }
        [Required]
        public int Suradnja { get; set; }
        [Required]
        public string Zakljucak { get; set; }

    }
}