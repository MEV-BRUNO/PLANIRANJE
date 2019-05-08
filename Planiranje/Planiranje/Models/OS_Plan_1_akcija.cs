using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
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
        [Required(ErrorMessage ="Obavezno polje")]
        [DisplayName("Opis akcije")]
        public string Opis_akcija { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]    
        [DisplayName("Potrebno sati")]
        public string Potrebno_sati { get; set; }        
        public int Br_sati { get; set; }
        [DisplayName("Siječanj")]
        [Required(ErrorMessage = "Obavezno polje")]
        [Range(0, int.MaxValue, ErrorMessage = "Vrijednost mora biti veća od 0")]
        public int Mj_1 { get; set; }
        [DisplayName("Veljača")]
        [Required(ErrorMessage = "Obavezno polje")]
        [Range(0, int.MaxValue, ErrorMessage = "Vrijednost mora biti veća od 0")]
        public int Mj_2 { get; set; }
        [DisplayName("Ožujak")]
        [Required(ErrorMessage = "Obavezno polje")]
        [Range(0, int.MaxValue, ErrorMessage = "Vrijednost mora biti veća od 0")]
        public int Mj_3 { get; set; }
        [DisplayName("Travanj")]
        [Required(ErrorMessage = "Obavezno polje")]
        [Range(0, int.MaxValue, ErrorMessage = "Vrijednost mora biti veća od 0")]
        public int Mj_4 { get; set; }
        [DisplayName("Svibanj")]
        [Required(ErrorMessage = "Obavezno polje")]
        [Range(0, int.MaxValue, ErrorMessage = "Vrijednost mora biti veća od 0")]
        public int Mj_5 { get; set; }
        [DisplayName("Lipanj")]
        [Required(ErrorMessage = "Obavezno polje")]
        [Range(0, int.MaxValue, ErrorMessage = "Vrijednost mora biti veća od 0")]
        public int Mj_6 { get;set;}
        [DisplayName("Srpanj")]
        [Required(ErrorMessage = "Obavezno polje")]
        [Range(0, int.MaxValue, ErrorMessage = "Vrijednost mora biti veća od 0")]
        public int Mj_7 { get; set; }
        [DisplayName("Kolovoz")]
        [Required(ErrorMessage = "Obavezno polje")]
        [Range(0, int.MaxValue, ErrorMessage = "Vrijednost mora biti veća od 0")]
        public int Mj_8 { get; set; }
        [DisplayName("Rujan")]
        [Required(ErrorMessage = "Obavezno polje")]
        [Range(0, int.MaxValue, ErrorMessage = "Vrijednost mora biti veća od 0")]
        public int Mj_9 { get; set; }
        [DisplayName("Listopad")]
        [Required(ErrorMessage = "Obavezno polje")]
        [Range(0, int.MaxValue, ErrorMessage = "Vrijednost mora biti veća od 0")]
        public int Mj_10 { get; set; }
        [DisplayName("Studeni")]
        [Required(ErrorMessage = "Obavezno polje")]
        [Range(0, int.MaxValue, ErrorMessage = "Vrijednost mora biti veća od 0")]
        public int Mj_11 { get; set; }
        [DisplayName("Prosinac")]
        [Required(ErrorMessage ="Obavezno polje")]
        [Range(0,int.MaxValue,ErrorMessage ="Vrijednost mora biti veća od 0")]
        public int Mj_12 { get; set; }
    }
}