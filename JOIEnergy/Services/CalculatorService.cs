using JOIEnergy.Domain;
using JOIEnergy.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JOIEnergy.Services
{
    public class CalculatorService : ICalculatorService
    {
        private readonly IMeterReadingService _meterReadingService;
        //private Dictionary<string, Supplier> _smartMeterToPricePlanAccounts;
        private readonly List<PricePlan> _pricePlans;

        public Dictionary<string, List<ElectricityReading>> _meterAssociatedReadings { get; set; }

        public CalculatorService(IMeterReadingService meterReadingService,
            Dictionary<string, List<ElectricityReading>> meterAssociatedReadings,
            List<PricePlan> pricePlans)
        {
            _meterReadingService = meterReadingService;
            _meterAssociatedReadings = meterAssociatedReadings;
            _pricePlans = pricePlans;
        }
        
        public decimal CalculateCost(List<ElectricityReading> electricityReadings, Supplier supplier, DateTime StartDate, DateTime EndDate)
        {
            var amount = 0m;
            var readings = electricityReadings.Where(r => r.Time >= StartDate && r.Time <= EndDate);
            if (readings.Count() > 0)
            {
                var avgUnits = (readings.Select(r => r.Reading)
                                                .Aggregate((reading, accumalator) => reading + accumalator)
                            / readings.Count());
                var timeElapsed = (readings.Max(r => r.Time) - readings.Min(r => r.Time)).TotalHours;
                var planCharge = _pricePlans.Where(p => p.EnergySupplier.Equals(supplier));
                amount = (avgUnits * planCharge.Last().UnitRate) / (decimal)timeElapsed;
            }
            return amount;
        }
    }
}
