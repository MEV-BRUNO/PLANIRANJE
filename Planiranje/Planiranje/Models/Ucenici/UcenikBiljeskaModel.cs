using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Planiranje.Models.Ucenici
{
    public class UcenikBiljeskaModel
    {
        public Ucenik_biljeska UcenikBiljeska { get; set; }
        public List<Obitelj> ListaObitelji { get; set; }
        public Ucenik Ucenik { get; set; }
        /// <summary>
        /// popis razrednih odjela u kojima je odabrani učenik upisani
        /// </summary>
        public List<RazredniOdjel> RazredniOdjeli { get; set; }
        public List<Mjesecna_biljeska> MjesecneBiljeske { get; set; }
        public Mjesecna_biljeska MjesecnaBiljeska { get; set; }
    }
}