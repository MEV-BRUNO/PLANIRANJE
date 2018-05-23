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
        public int Potrebno_sati { get; set; }
        [Required]
        public int Br_sati { get; set; }
        [Required]
        public int Deveti_mjesec { get; set; }
        [Required]
        public int Deseti_mjesec { get; set; }
        [Required]
        public int Jedanaesti_mjesec { get; set; }
        [Required]
        public int Dvanaesti_mjesec { get; set; }
        [Required]
        public int Prvi_mjesec { get; set; }
        [Required]
        public int Drugi_mjesec {get;set;}
        [Required]
        public int Treci_mjesec { get; set; }
        [Required]
        public int Cetvrti_mjesec { get; set; }
        [Required]
        public int Peti_Mjesec { get; set; }
        [Required]
        public int Sesti_mjesec { get; set; }
        [Required]
        public int Sedmi_mjesec { get; set; }
        [Required]
        public int Osmi_mjesec { get; set; }


    }
}