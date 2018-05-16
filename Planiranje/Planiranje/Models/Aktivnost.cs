using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planiranje.Models
{
    public class Aktivnost
    {   
        [Required]
        public int Id_aktivnost { get; set; }
        [Required]
        public string Naziv { get; set; }
    }
}