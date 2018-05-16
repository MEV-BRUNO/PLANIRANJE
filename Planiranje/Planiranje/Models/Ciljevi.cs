using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planiranje.Models
{
    public class Ciljevi
    {
        [Required]
        public int ID_cilj { get; set; }
        [Required]
        public string Naziv { get; set; }
    }
}