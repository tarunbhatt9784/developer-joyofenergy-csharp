using JOIEnergy.Domain;
using JOIEnergy.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JOIEnergy.Services
{
    public interface ICalculatorService
    {
        decimal CalculateCost(List<ElectricityReading> electricityReadings, Supplier supplier, DateTime StartDate, DateTime EndDate);
    }
}
