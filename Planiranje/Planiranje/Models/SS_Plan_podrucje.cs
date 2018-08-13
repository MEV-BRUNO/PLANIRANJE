﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
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
		[Required(ErrorMessage = "Obavezno polje.")]
		[DisplayName("Opis")]
		public string Opis_podrucje { get; set; }
		[Required(ErrorMessage = "Obavezno polje.")]
		[DisplayName("Svrha")]
		public string Svrha { get; set; }
		[Required(ErrorMessage = "Obavezno polje.")]
		[DisplayName("Zadaća")]
		public string Zadaca { get; set; }
		[Required(ErrorMessage = "Obavezno polje.")]
		[DisplayName("Sadržaj")]
		public string Sadrzaj { get; set; }
		[Required(ErrorMessage = "Obavezno polje.")]
		[DisplayName("Oblici")]
		public string Oblici { get; set; }
		[Required(ErrorMessage = "Obavezno polje.")]
		[DisplayName("Suradnici")]
		public string Suradnici { get; set; }
		[Required(ErrorMessage = "Obavezno polje.")]
		[DisplayName("Mjesto")]
		public string Mjesto { get; set; }
		[Required(ErrorMessage = "Obavezno polje.")]
		[DisplayName("Vrijeme")]
		[DataType(DataType.DateTime)]
        public DateTime Vrijeme { get; set; }
		[Required(ErrorMessage = "Obavezno polje.")]
		[DisplayName("Ishodi")]
		public string Ishodi { get; set; }
		[Required(ErrorMessage = "Obavezno polje.")]
		[DisplayName("Sati")]
		public int Sati { get; set; }
    }
}