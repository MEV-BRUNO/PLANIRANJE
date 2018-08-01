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
        [Required]
        public int Id_plan { get; set; }
        [Required]
        public int Id_podrucje { get; set; }        
        [Required]
        [DisplayName("Opis aktivnosti")]
        public int Opis_aktivnost { get; set; }
        [Required]
        public int Red_broj_aktivnost { get; set; }
        [Required]
        [DisplayName("Potrebno sati")]
        public string Potrebno_sati { get; set; }
        [Required]
        public int Br_sati { get; set; }
		[Required]
        [DisplayName("Siječanj")]
        public int Mj_1 { get; set; }
		[Required]
        [DisplayName("Veljača")]
        public int Mj_2 { get; set; }
		[Required]
        [DisplayName("Ožujak")]
        public int Mj_3 { get; set; }
		[Required]
        [DisplayName("Travanj")]
        public int Mj_4 { get; set; }
		[Required]
        [DisplayName("Svibanj")]
        public int Mj_5 { get; set; }
		[Required]
        [DisplayName("Lipanj")]
        public int Mj_6 { get; set; }
		[Required]
        [DisplayName("Srpanj")]
        public int Mj_7 { get; set; }
		[Required]
        [DisplayName("Kolovoz")]
        public int Mj_8 { get; set; }
		[Required]
        [DisplayName("Rujan")]
        public int Mj_9 { get; set; }
		[Required]
        [DisplayName("Listopad")]
        public int Mj_10 { get; set; }
		[Required]
        [DisplayName("Studeni")]
        public int Mj_11 { get; set; }
		[Required]
        [DisplayName("Prosinac")]
        public int Mj_12 { get; set; }

	}
}