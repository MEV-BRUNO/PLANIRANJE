using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Planiranje.Models
{
    public class Godisnji_plan
    {
        public int Id_god { get; set; }
        public int Ak_godina { get; set; }
        public int Id_pedagog { get; set; }
        public int Br_radnih_dana { get; set; }
        public int Br_dana_godina_odmor { get; set; }
        public int Ukupni_rad_dana { get; set; }
        public int God_fond_sati { get; set; }
    }
}