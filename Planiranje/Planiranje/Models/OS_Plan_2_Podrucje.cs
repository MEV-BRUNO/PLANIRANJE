using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planiranje.Models
{
    public class OS_Plan_2_podrucje
    {
        [Required]
        public int Id_plan { get; set; }
        [Required]
        public int Red_br_podrucje { get; set; }
        [Required]
        public string Opis_podrucje { get; set; }
        [Required]
        public int Cilj { get; set; }
        [Required]
        public int Zadaci { get; set; }
        [Required]
        public int Subjekti { get; set; }
        [Required]
        public int Oblici { get; set; }
        [Required]        
        public string Vrijeme { get; set; }
        [Required]        
        public int Id_glavni_plan { get; set; }
    }
}
