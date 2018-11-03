using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planiranje.Models
{
    public class Mjesecni_detalji
	{
        [Key]
		[Required]
		public int ID { get; set; }
		[Required]        
		public int ID_plan { get; set; }			
		[DisplayName("Područje rada")]
		public string Podrucje { get; set; }		
		[DisplayName("Aktivnost")]
		public string Aktivnost { get; set; }		
		[DisplayName("Suradnici")]
		public string Suradnici { get; set; }		
		[DataType(DataType.Date)]
		[DisplayName("Vrijeme izvršenja")]
		[DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
		public DateTime Vrijeme { get; set; }		
		[DisplayName("Vrijeme rada")]		
		public int Br_sati { get; set; }		
		[DisplayName("Bilješke")]
		public string Biljeska { get; set; }
    }
}