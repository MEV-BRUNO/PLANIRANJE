﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planiranje.Models.Ucenici
{
    public class Pracenje_ucenika
    {
        [Key]
        public int Id { get; set; }
        public int Id_ucenik_razred { get; set; }
        public int Id_pedagog { get; set; }
        public string Razlog { get; set; }
        public string Inic_Procjena_ucenik { get; set; }
        public string Inic_Procjena_roditelj { get; set; }
        public string Inic_Procjena_razrednik { get; set; }
        public string Soc_uvjeti { get; set; }
        public string Ucenje { get; set; }
        public string Soc_vjestine { get; set; }
        public string Zakljucak { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Početak praćenja")]
        public DateTime Pocetak_pracenja { get; set; }
    }
}