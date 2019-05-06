using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planiranje.Models
{
    public class OS_Plan_1_akcija
    {
        [Key]        
        public int Id { get; set; }        
        public int Id_aktivnost { get; set; }        
        public int Red_br_akcija { get; set; }
        [Required]
        public string Opis_akcija { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]        
        public string Potrebno_sati { get; set; }        
        public int Br_sati { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        [Range(0, int.MaxValue, ErrorMessage = "Vrijednost mora biti veća od 0")]
        public int Mj_1 { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        [Range(0, int.MaxValue, ErrorMessage = "Vrijednost mora biti veća od 0")]
        public int Mj_2 { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        [Range(0, int.MaxValue, ErrorMessage = "Vrijednost mora biti veća od 0")]
        public int Mj_3 { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        [Range(0, int.MaxValue, ErrorMessage = "Vrijednost mora biti veća od 0")]
        public int Mj_4 { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        [Range(0, int.MaxValue, ErrorMessage = "Vrijednost mora biti veća od 0")]
        public int Mj_5 { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        [Range(0, int.MaxValue, ErrorMessage = "Vrijednost mora biti veća od 0")]
        public int Mj_6 { get;set;}
        [Required(ErrorMessage = "Obavezno polje")]
        [Range(0, int.MaxValue, ErrorMessage = "Vrijednost mora biti veća od 0")]
        public int Mj_7 { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        [Range(0, int.MaxValue, ErrorMessage = "Vrijednost mora biti veća od 0")]
        public int Mj_8 { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        [Range(0, int.MaxValue, ErrorMessage = "Vrijednost mora biti veća od 0")]
        public int Mj_9 { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        [Range(0, int.MaxValue, ErrorMessage = "Vrijednost mora biti veća od 0")]
        public int Mj_10 { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        [Range(0, int.MaxValue, ErrorMessage = "Vrijednost mora biti veća od 0")]
        public int Mj_11 { get; set; }
        [Required(ErrorMessage ="Obavezno polje")]
        [Range(0,int.MaxValue,ErrorMessage ="Vrijednost mora biti veća od 0")]
        public int Mj_12 { get; set; }
    }
}