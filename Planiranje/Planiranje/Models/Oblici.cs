using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planiranje.Models
{
    public class Oblici
    {
        [Required]
        public int Id_oblici { get; set; }
        [Required]
        public string Naziv { get; set; }

    }
}