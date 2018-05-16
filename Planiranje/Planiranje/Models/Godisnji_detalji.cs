using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planiranje.Models
{
    public class Godisnji_detalji
    {
        [Required]
        public int Id_god { get; set; }
        [Required]
        public int Mjesec { get; set; }
        [Required]
        public string Naziv_mjesec { get; set; }
        [Required]
        public int Ukupno_dana { get; set; }
        [Required]
        public int Radnih_dana { get; set; }
        [Required]
        public int Subota_dana { get; set; }
        [Required]
        public int Blagdana_dana { get; set; }
        [Required]
        public int Nastavnih_dana { get; set; }
        [Required]
        public int Praznika_dana { get; set; }
        [Required]
        public int Br_sati { get; set; }
        [Required]
        public int Odmor_dana { get; set; }
        [Required]
        public int Odmor_sati { get; set; }
        [Required]
        public int Mj_fond_sati { get; set; }
        [Required]
        public int Br_rad_dana_sk_god { get; set; }
        [Required]
        public int Br_dana_god_odmor { get; set; }
        [Required]
        public int Ukupno_rad_dana { get; set; }
        [Required]
        public int God_fond_sati { get; set; }
    }
}