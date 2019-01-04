using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planiranje.Models.Ucenici
{
    public class Ucenik_biljeska
    {
        [Key]
        public int Id_biljeska { get; set; }
        public int Id_ucenik_razred { get; set; }
        [DisplayName("Inicijalni podaci")]
        public string Inicijalni_podaci { get; set; }
        [DisplayName("Zapažanje")]
        public string Zapazanje { get; set; }
    }
}