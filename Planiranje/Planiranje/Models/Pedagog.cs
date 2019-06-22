using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planiranje.Models
{
	public class Pedagog
	{
		[Key]        
        public int Id_Pedagog { get; set; }
        [Required(ErrorMessage ="Ime je obavezno")]
        public string Ime { get; set; }
        [Required(ErrorMessage ="Prezime je obavezno")]
        public string Prezime { get; set; }
		[Required(ErrorMessage = "Email adresa je obavezna.")]
		[DataType(DataType.EmailAddress)]
        public string Email { get; set; }
		[DisplayName("Password")]
		[Required(ErrorMessage = "Lozinka je obavezna.")]
		[StringLength(12, MinimumLength = 6, ErrorMessage = "Lozinka mora biti između 6 i 12 znakova")]
		public string Lozinka { get; set; }		
        [DataType(DataType.DateTime)]        
        public DateTime Licenca { get; set; } //datum trajanja pristupa       
        public bool Aktivan { get; set; }
        [Required(ErrorMessage ="Titula je obavezna")]
        public string Titula { get; set; }
    }
}