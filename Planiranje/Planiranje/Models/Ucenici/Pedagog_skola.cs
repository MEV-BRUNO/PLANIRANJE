using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planiranje.Models.Ucenici
{
    public class Pedagog_skola
    {
        [Key]
        public int Id { get; set; }
        public int Id_pedagog { get; set; }
        public int Id_skola { get; set; }
    }
}