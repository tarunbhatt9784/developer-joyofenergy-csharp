using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JOIEnergy.Domain
{
    public class Bill
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Amount { get; set; }
        public DateTime? BillingDate { get; set; }
        public DateTime? LastDate { get; set; }
    }
}
