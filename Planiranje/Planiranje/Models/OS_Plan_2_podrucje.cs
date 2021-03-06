﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planiranje.Models
{
    public class OS_Plan_2_podrucje
    {
        [Key]        
        public int Id_plan { get; set; }        
        public int Red_br_podrucje { get; set; }
        [Required(ErrorMessage ="Obavezno polje")]
        [DisplayName("Posao")]
        public string Opis_podrucje { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        [DisplayName("Ciljevi")]
        public string Cilj { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        [DisplayName("Zadaci")]
        public string Zadaci { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        public string Subjekti { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        [DisplayName("Oblik/metoda rada")]
        public string Oblici { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]        
        [DisplayName("Vrijeme realizacije")]
        public string Vrijeme { get; set; }              
        public int Id_glavni_plan { get; set; }   
        [Required(ErrorMessage ="Obavezno polje")]
        [Range(0,int.MaxValue,ErrorMessage ="Vrijednost mora biti veća ili jednaka 0")]
        public int Sati { get; set; }
    }
}
