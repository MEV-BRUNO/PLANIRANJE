using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planiranje.Models
{
    public class Mjesecni_plan
    {
		public int Red_br { get; set; }
		[Required]
        public int ID_plan { get; set; }
        [Required]
        public int ID_pedagog { get; set; }
		public int Id_godina { get; set; }
		[Required]
		[DisplayName("Ak. godina")]
		public string Ak_godina { get; set; }
        [Required]
		[DisplayName("Naziv plana")]
		public string Naziv { get; set; }
        [Required]
		[DisplayName("Opis plana")]
		public string Opis { get; set; }
    }
}