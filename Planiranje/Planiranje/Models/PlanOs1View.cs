using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Planiranje.Models
{
    public class PlanOs1View
    {
        public OS_Plan_1 OsPlan1 { get; set; }
        public List<OS_Plan_1_podrucje> OsPlan1Podrucje { get; set; }
        public List<OS_Plan_1_aktivnost> OsPlan1Aktivnost { get; set; }
        public List<OS_Plan_1_akcija> OsPlan1Akcija { get; set; }      
                
    }
}