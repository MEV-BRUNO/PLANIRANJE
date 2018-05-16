using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planiranje.Models
{
    public class Godisnji_plan
    {
        [Required]
        public int Id_god { get; set; }
        [Required]
        public int Ak_godina { get; set; }
        [Required]
        public int Id_pedagog { get; set; }
        [Required]
        public int Br_radnih_dana { get; set; }
        [Required]
        public int Br_dana_godina_odmor { get; set; }
        [Required]
        public int Ukupni_rad_dana { get; set; }
        [Required]
        public int God_fond_sati { get; set; }
    }
}