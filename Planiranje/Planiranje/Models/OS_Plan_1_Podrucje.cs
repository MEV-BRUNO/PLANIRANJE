using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Planiranje.Models
{
    public class OS_Plan_1_podrucje
    {
        [Key]        
        public int Id_plan { get; set; }        
        public int Id_glavni_plan { get; set; }        
        public int Red_br_podrucje { get; set; }
        [Required(ErrorMessage ="Ovo je obavezno polje")]
        [DisplayName("Opis područja")]
        public int Opis_Podrucje { get; set; }
        [Required(ErrorMessage = "Ovo je obavezno polje")]
        [DisplayName("Potrebno sati")]
        public string Potrebno_sati { get; set; }
        [Required(ErrorMessage = "Ovo je obavezno polje")]
        [DisplayName("Ciljevi")]
        public string Cilj { get; set; }
        [Required]
        public int Br_sati { get; set; }
		[Required]
		public int Mj_1 { get; set; }
		[Required]
		public int Mj_2 { get; set; }
		[Required]
		public int Mj_3 { get; set; }
		[Required]
		public int Mj_4 { get; set; }
		[Required]
		public int Mj_5 { get; set; }
		[Required]
		public int Mj_6 { get; set; }
		[Required]
		public int Mj_7 { get; set; }
		[Required]
		public int Mj_8 { get; set; }
		[Required]
		public int Mj_9 { get; set; }
		[Required]
		public int Mj_10 { get; set; }
		[Required]
		public int Mj_11 { get; set; }
		[Required]
		public int Mj_12 { get; set; }

	}
}