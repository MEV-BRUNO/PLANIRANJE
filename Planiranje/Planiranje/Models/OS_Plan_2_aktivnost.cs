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
        public int Red_br_podrucje { get; set; }
        [Required]
        public int Red_br_aktivnost { get; set; }
        [Required]
        public string Opis_aktivnost { get; set; }
        [Required]
        public int Cilj { get; set; }
        [Required]
        public int Zadaci { get; set; }
        [Required]
        public int Subjekti { get; set; }
        [Required]
        public int Oblici { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Vrijeme { get; set; }
        [Required]
        public int Sati { get; set; }
    
}
}