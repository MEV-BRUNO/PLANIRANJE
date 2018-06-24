using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planiranje.Models
{
    public class Subjekti
    {
        [Required]
        public int ID_subjekt { get; set; }
        [Required]
        public string Naziv { get; set; }

        internal bool CreateSubjekti(Subjekti subjekti)
        {
            throw new NotImplementedException();
        }

        internal Subjekti ReadSubjekti(int id)
        {
            throw new NotImplementedException();
        }

        internal bool UpdateSubjekti(Subjekti subjekti)
        {
            throw new NotImplementedException();
        }

        internal bool DeleteSubjekti(int iD_subjekt)
        {
            throw new NotImplementedException();
        }
    }
}