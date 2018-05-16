using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planiranje.Models
{
    public class SS_Plan_podrucje
    {
        [Required]
        public int ID_plan { get; set; }
        [Required]
        public int Red_br_podrucje { get; set; }
        [Required]
        public int Opis_podrucje { get; set; }
        [Required]
        public string Svrha { get; set; }
        [Required]
        public string Zadaca { get; set; }
        [Required]
        public string Sadrzaj { get; set; }
        [Required]
        public int Oblici { get; set; }
        [Required]
        public string Suradnici { get; set; }
        [Required]
        public string Mjesto { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Vrijeme { get; set; }
        [Required]
        public string Ishodi { get; set; }
        [Required]
        public int Sati { get; set; }
    }
}