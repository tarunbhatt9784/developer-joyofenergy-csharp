using JOIEnergy.Domain;
using JOIEnergy.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JOIEnergy.Services
{
    public class BillingService : IBillingService
    {
        private Dictionary<string, Supplier> _smartMeterToPricePlanAccounts;
        private readonly List<PricePlan> _pricePlans;
        private readonly ICalculatorService _calculatorService;

        public Dictionary<string, List<ElectricityReading>> _meterAssociatedReadings { get; set; }

        public BillingService(Dictionary<string, List<ElectricityReading>> meterAssociatedReadings,
            Dictionary<string, Supplier> smartMeterToPricePlanAccounts,
            List<PricePlan> pricePlans,
            ICalculatorService calculatorService)
        {
            _meterAssociatedReadings = meterAssociatedReadings;
            _smartMeterToPricePlanAccounts = smartMeterToPricePlanAccounts;
            _pricePlans = pricePlans;
            _calculatorService = calculatorService;
        }

        public Bill GenerateBill(string meterId, DateTime? StartDate, DateTime? EndDate)
        {
            Bill bill = new Bill();
            var electricityReadings = new List<ElectricityReading>();
            Supplier supplier = new Supplier();
            if (!string.IsNullOrEmpty(meterId) &&
                _meterAssociatedReadings.TryGetValue(meterId, out electricityReadings) &&
                electricityReadings.Count > 0 &&
                _smartMeterToPricePlanAccounts.TryGetValue(meterId, out supplier) &&
                _pricePlans.Any(p => p.EnergySupplier.Equals(supplier)))
            {
                bill.StartDate = (DateTime)(StartDate == null ? electricityReadings.Min(r => r.Time) : StartDate);
                bill.EndDate = (DateTime)(EndDate == null ? electricityReadings.Max(r => r.Time) : EndDate);
                bill.Amount = _calculatorService.CalculateCost(electricityReadings, supplier, bill.StartDate,bill.EndDate);
                bill.BillingDate = DateTime.UtcNow;
                bill.LastDate = bill.Amount !=0? bill.BillingDate + (new TimeSpan(45, 0, 0, 0)):null;
            }
            return bill;
        }
    }
}
