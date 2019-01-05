using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Planiranje.Models.Ucenici
{
    public class PromatranjeUcenikaModel
    {
        public Ucenik Ucenik { get; set; }
        public RazredniOdjel Razred { get; set; }
        public List<RazredniOdjel> RazredniOdjeli { get; set; }
        public Ucenik_razred UcenikRazred { get; set; }
        public Promatranje_ucenika PromatranjeUcenika { get; set; }
        public List<Promatranje_ucenika> PromatranjaUcenika { get; set; }
    }
}