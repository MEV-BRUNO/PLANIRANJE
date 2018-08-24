using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planiranje.Models
{
    public class OS_Plan_2_aktivnost
    {
        [Required]
        public int Id_plan { get; set; }
        [Required]
        public int Id_podrucje { get; set; }
        [Required]
        public int Red_br_aktivnost { get; set; }
        [Required]
        public string Opis_aktivnost { get; set; }           
    }
}