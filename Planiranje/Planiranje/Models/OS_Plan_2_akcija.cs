using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planiranje.Models
{
    public class OS_Plan_2_akcija
    {
        [Required]
        public int Id_plan { get; set; }        
        [Required]
        public int Id_aktivnost { get; set; }
        [Required]
        public int Red_br_akcija { get; set; }
        [Required]
        public string Opis_akcija { get; set; }
        [Required]
        public int Sati { get; set; }
    }
}