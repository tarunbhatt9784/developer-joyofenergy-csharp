using JOIEnergy.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JOIEnergy.Services
{
    public interface IBillingService
    {
        Bill GenerateBill(string meterId,DateTime? StartDate, DateTime? EndDate);
    }
}
