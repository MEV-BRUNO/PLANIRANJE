using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planiranje.Models
{
    public class Aktivnost_akcija
    {
        [Required]
        public int Id_akcija { get; set; }
        [Required]
        public string Naziv { get; set; }
        [Required]
        public int Id_aktivnost { get; set; }
    }
}