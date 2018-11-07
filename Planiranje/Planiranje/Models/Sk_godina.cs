using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planiranje.Models
{
    public class Sk_godina
    {
        [Key]
        [Required]
        public int Sk_Godina { get; set; }
    }
}