using JOIEnergy.Domain;
using JOIEnergy.Enums;
using JOIEnergy.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace JOIEnergy.Tests
{
    public class CalculatorServiceTest
    {
        private MeterReadingService _meterReadingService;
        private static string SMART_METER_ID = "smart-meter-id";
        private List<PricePlan> _pricePlans;
        Dictionary<String, Supplier> _smartMeterToPricePlanAccounts;
        private Dictionary<string,List<ElectricityReading>> _meterAssociatedReadings = 
            new Dictionary<string,List<ElectricityReading>>();
        private List<ElectricityReading> _meterReadings;
        private ICalculatorService _calculatorService;
        public CalculatorServiceTest()
        {
            _meterReadingService = new MeterReadingService(new Dictionary<string, List<ElectricityReading>>());

            _meterReadings = new List<ElectricityReading>() {
                new ElectricityReading() { Time = new DateTime(2022,01,01,0,0,0), Reading = 60m },
                new ElectricityReading() { Time = new DateTime(2022,01,01,10,00,0), Reading = 50m },
                new ElectricityReading() { Time = new DateTime(2022,01,01,20,00,0), Reading = 30m },
                new ElectricityReading() { Time = new DateTime(2022,01,02,16,40,0), Reading = 20m },
            };
            _meterReadingService.StoreReadings(SMART_METER_ID, _meterReadings);
            _meterAssociatedReadings.Add(SMART_METER_ID, _meterReadingService.GetReadings(SMART_METER_ID));
            _pricePlans = new List<PricePlan>() {
                new PricePlan() { EnergySupplier = Supplier.DrEvilsDarkEnergy, UnitRate = 10, PeakTimeMultiplier = new List<PeakTimeMultiplier>() },
                new PricePlan() { EnergySupplier = Supplier.TheGreenEco, UnitRate = 2, PeakTimeMultiplier = new List<PeakTimeMultiplier>() },
                new PricePlan() { EnergySupplier = Supplier.PowerForEveryone, UnitRate = 1, PeakTimeMultiplier = new List<PeakTimeMultiplier>() }
            };
        }
        [Theory]
        [InlineData("smart-meter-id", Supplier.DrEvilsDarkEnergy)]
        public void CalculateCostBetweebTwoValidDates(string meterId, Supplier supplier)
        {
            _smartMeterToPricePlanAccounts = new Dictionary<string, Supplier>()
            {
                { SMART_METER_ID,supplier}
            };
           _calculatorService = new CalculatorService(_meterReadingService, _meterAssociatedReadings, _pricePlans);
            decimal cost = _calculatorService.CalculateCost(_meterReadings, supplier, new DateTime(2022, 01, 01, 0, 0, 0), new DateTime(2023, 01, 01, 0, 0, 0));
            Assert.Equal(Math.Round(cost), 10);
        }
    }
}
