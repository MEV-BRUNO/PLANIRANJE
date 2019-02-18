﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planiranje.Models.Ucenici
{
    public class Roditelj_biljeska
    {
        public int Id { get; set; }
        public int Id_ucenik_razred { get; set; }
        public int Id_pedagog { get; set; }
        [Required(ErrorMessage ="Obavezno polje")]
        [Range(1,int.MaxValue,ErrorMessage ="Obavezno polje")]
        public int Id_roditelj { get; set; }
        [Required(ErrorMessage ="Obavezno polje")]
        [MaxLength(100,ErrorMessage ="Maksimalan broj znakova je 100")]
        public string Naslov { get; set; }
        public string Rujan { get; set; }
        public string Listopad { get; set; }
        public string Studeni { get; set; }
        public string Prosinac { get; set; }
        public string Sijecanj { get; set; }
        public string Veljaca { get; set; }
        public string Ozujak { get; set; }
        public string Travanj { get; set; }
        public string Svibanj { get; set; }
        public string Lipanj { get; set; }
        public string Zakljucak1 { get; set; }
        public string Zakljucak2 { get; set; }
        public string Zapazanje { get; set; }
    }
}