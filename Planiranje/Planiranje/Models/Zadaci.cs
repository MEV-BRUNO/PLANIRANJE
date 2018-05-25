using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planiranje.Models
{
    public class Zadaci
    {
        [Required]
        public int ID_zadatak { get; set; }
        [Required]
        public string Naziv { get; set; }
    }
}