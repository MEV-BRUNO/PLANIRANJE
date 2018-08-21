using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planiranje.Models
{
    public class Podrucje_rada
    {
        [Required(ErrorMessage ="Obavezno polje")]
        public int Id_podrucje { get; set; }
		[Required(ErrorMessage = "Obavezno polje")]
		public string Naziv { get; set; }
    }
}