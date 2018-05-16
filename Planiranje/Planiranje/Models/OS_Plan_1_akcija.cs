using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planiranje.Models
{
    public class OS_Plan_1_akcija
    {
        [Required]
        public int Id_plan { get; set; }
        [Required]
        public int Red_br_podrucje { get; set; }
        [Required]
        public int Red_br_aktivnost { get; set; }
        [Required]
        public int Red_br_akcija { get; set; }
        [Required]
        public string Opis_akcija { get; set; }
        [Required]
        public int Red_broj { get; set; }
        [Required]
        public double Potrebno_sati { get; set; }
        [Required]
        public double Br_sati { get; set; }
        [Required]
        public double Deveti_mjesec { get; set; }
        [Required]
        public double Deseti_mjesec { get; set; }
        [Required]
        public double Jedanaesti_mjesec { get; set; }
        [Required]
        public double Dvanaesti_mjesec { get; set; }
        [Required]
        public double Prvi_mjesec { get; set; }
        [Required]
        public double Drugi_mjesec{get;set;}
        [Required]
        public double Treci_mjesec { get; set; }
        [Required]
        public double Cetvrti_mjesec { get; set; }
        [Required]
        public double Peti_Mjesec { get; set; }
        [Required]
        public double Sesti_mjesec { get; set; }
        [Required]
        public double Sedmi_mjesec { get; set; }
        [Required]
        public double Osmi_mjesec { get; set; }


    }
}