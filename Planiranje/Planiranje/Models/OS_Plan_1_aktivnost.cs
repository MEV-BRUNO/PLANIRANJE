using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planiranje.Models
{
    public class OS_Plan_1_aktivnost
    {
        [Key]        
        public int Id_plan { get; set; }        
        public int Id_podrucje { get; set; }        
        [Required(ErrorMessage = "Ovo je obavezno polje")]
        [DisplayName("Opis aktivnosti")]        
        public int Opis_aktivnost { get; set; }        
        public int Red_broj_aktivnost { get; set; }
        [Required(ErrorMessage = "Ovo je obavezno polje")]
        [DisplayName("Potrebno sati")]
        public string Potrebno_sati { get; set; }
        [Required]
        public int Br_sati { get; set; }        
        [DisplayName("Siječanj")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Vrijednost mora biti jednaka ili veća od 0")]
        public int Mj_1 { get; set; }		
        [Range(0, Int32.MaxValue, ErrorMessage = "Vrijednost mora biti jednaka ili veća od 0")]
        [DisplayName("Veljača")]
        public int Mj_2 { get; set; }		
        [Range(0, Int32.MaxValue, ErrorMessage = "Vrijednost mora biti jednaka ili veća od 0")]
        [DisplayName("Ožujak")]
        public int Mj_3 { get; set; }		
        [Range(0, Int32.MaxValue, ErrorMessage = "Vrijednost mora biti jednaka ili veća od 0")]
        [DisplayName("Travanj")]
        public int Mj_4 { get; set; }		
        [Range(0, Int32.MaxValue, ErrorMessage = "Vrijednost mora biti jednaka ili veća od 0")]
        [DisplayName("Svibanj")]
        public int Mj_5 { get; set; }		
        [Range(0, Int32.MaxValue, ErrorMessage = "Vrijednost mora biti jednaka ili veća od 0")]
        [DisplayName("Lipanj")]
        public int Mj_6 { get; set; }		
        [Range(0, Int32.MaxValue, ErrorMessage = "Vrijednost mora biti jednaka ili veća od 0")]
        [DisplayName("Srpanj")]
        public int Mj_7 { get; set; }		
        [Range(0, Int32.MaxValue, ErrorMessage = "Vrijednost mora biti jednaka ili veća od 0")]
        [DisplayName("Kolovoz")]
        public int Mj_8 { get; set; }		
        [Range(0, Int32.MaxValue, ErrorMessage = "Vrijednost mora biti jednaka ili veća od 0")]
        [DisplayName("Rujan")]
        public int Mj_9 { get; set; }		
        [Range(0, Int32.MaxValue, ErrorMessage = "Vrijednost mora biti jednaka ili veća od 0")]
        [DisplayName("Listopad")]
        public int Mj_10 { get; set; }		
        [Range(0, Int32.MaxValue, ErrorMessage = "Vrijednost mora biti jednaka ili veća od 0")]
        [DisplayName("Studeni")]
        public int Mj_11 { get; set; }		
        [Range(0, Int32.MaxValue, ErrorMessage = "Vrijednost mora biti jednaka ili veća od 0")]
        [DisplayName("Prosinac")]
        public int Mj_12 { get; set; }
	}
}