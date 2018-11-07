using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planiranje.Models
{
    public class Mjesecni_detalji
    {
        [Required]
        public int ID_plan { get; set; }
        [Required]
        public int Red_br { get; set; }
        [Required]
        public int Podrucje { get; set; }
        [Required]
        public int Aktivnost { get; set; }
        [Required]
        public string Suradnici { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Vrijeme { get; set; }
        [Required]
        public int Br_sati { get; set; }
        [Required]
        public string Biljeska { get; set; }
    }
}