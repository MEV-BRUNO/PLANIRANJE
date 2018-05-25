using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planiranje.Models
{
    public class Podrucje_rada
    {
        [Required]
        public int Id_podrucje { get; set; }
        [Required]
        public string Naziv { get; set; }
    }
}