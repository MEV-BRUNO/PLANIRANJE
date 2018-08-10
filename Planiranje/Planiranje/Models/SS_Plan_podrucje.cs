using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planiranje.Models
{
    public class SS_Plan_podrucje
    {
		public int Red_br { get; set; }
		[Required]
		public int Id { get; set; }
		[Required]
        public int ID_plan { get; set; }
        [Required]
        public string Opis_podrucje { get; set; }
        [Required]
        public string Svrha { get; set; }
        [Required]
        public string Zadaca { get; set; }
        [Required]
        public string Sadrzaj { get; set; }
        [Required]
        public string Oblici { get; set; }
        [Required]
        public string Suradnici { get; set; }
        [Required]
        public string Mjesto { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Vrijeme { get; set; }
        [Required]
        public string Ishodi { get; set; }
        [Required]
        public int Sati { get; set; }
    }
}