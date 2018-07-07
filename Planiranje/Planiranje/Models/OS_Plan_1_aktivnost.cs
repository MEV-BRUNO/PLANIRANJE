using System;
using System.Collections.Generic;
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
        public int Red_broj_podrucje { get; set; }
        [Required]
        public int Red_broj_aktivnost { get; set; }
        [Required]
        public string Opis_aktivnost { get; set; }
        [Required]
        public int Red_br { get; set; }
        [Required]
        public int Potrebno_sati { get; set; }
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