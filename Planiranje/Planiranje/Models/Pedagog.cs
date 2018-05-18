using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planiranje.Models
{
	public class Pedagog
	{
		[Key]
        [Required]
        public int Id_Pedagog { get; set; }
        [Required]
        public string Ime { get; set; }
        [Required]
        public string Prezime { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
		public string Lozinka { get; set; }
		[Required]
        [DataType(DataType.DateTime)]
        public DateTime Licenca { get; set; } //datum trajanja pristupa
        [Required]
        public int Id_skola { get; set; }
        [Required]
        public bool Aktivan { get; set; }
        [Required]
        public string Titula { get; set; }
    }
}