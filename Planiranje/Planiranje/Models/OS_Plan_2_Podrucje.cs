using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planiranje.Models
{
    public class OS_Plan_2_Podrucje
    {
        [Required]
        public int Id_plan { get; set; }
        [Required]
        public int Red_br_podrucje { get; set; }
        [Required]
        public int Opis_podrucje { get; set; }
        [Required]
        public string Cilj { get; set; }
        [Required]
        public string Zadaci { get; set; }
        [Required]
        public string Subjekti { get; set; }
        [Required]
        public string Oblici { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Vrijeme { get; set; }
        [Required]        
        public int Sati { get; set; }
    }
}
