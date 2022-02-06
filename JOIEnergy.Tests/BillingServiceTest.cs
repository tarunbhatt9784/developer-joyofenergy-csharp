using JOIEnergy.Domain;
using JOIEnergy.Enums;
using JOIEnergy.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace JOIEnergy.Tests
{
    public class BillingServiceTest
    {
        private MeterReadingService _meterReadingService;
        private static string SMART_METER_ID = "smart-meter-id";
        private List<PricePlan> _pricePlans;
        Dictionary<String, Supplier> _smartMeterToPricePlanAccounts;
        private IBillingService _billingService;
        private Dictionary<string,List<ElectricityReading>> _meterAssociatedReadings = 
            new Dictionary<string,List<ElectricityReading>>();
        private List<ElectricityReading> _meterReadings;
        private ICalculatorService _calculatorService;
        public BillingServiceTest()
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
        [InlineData(null,Supplier.NullSupplier)]
        [InlineData("invalid-meter-id",Supplier.NullSupplier)]
        [InlineData("smart-meter-id", Supplier.NullSupplier)]
        public void CalculateBillingBetweenTwoDatesWithInvalidArgs(string meterId, Supplier supplier)
        {
            _smartMeterToPricePlanAccounts = new Dictionary<string, Supplier>()
            {
                { SMART_METER_ID,supplier}
            };
           _calculatorService = new CalculatorService(_meterReadingService, _meterAssociatedReadings, _pricePlans);
            _billingService = new BillingService(_meterAssociatedReadings, _smartMeterToPricePlanAccounts, _pricePlans,_calculatorService);
            Bill bill = _billingService.GenerateBill(meterId,null,null);
            Assert.Equal(bill.Amount, 0);
        }

        [Theory]
        [InlineData(null, null, 10, "01/01/2022 12:00:00 AM", "02/01/2022 04:40:00 PM")]
        [InlineData("", "", 10, "01/01/2022 12:00:00 AM", "02/01/2022 04:40:00 PM")]
        [InlineData("", null, 10, "01/01/2022 12:00:00 AM", "02/01/2022 04:40:00 PM")]
        [InlineData(null, "", 10, "01/01/2022 12:00:00 AM", "02/01/2022 04:40:00 PM")]
        [InlineData("01/01/2022 12:00:00 AM", null, 10, "01/01/2022 12:00:00 AM", "02/01/2022 04:40:00 PM")]
        [InlineData("01/01/2022 12:00:00 AM", "", 10, "01/01/2022 12:00:00 AM", "02/01/2022 04:40:00 PM")]
        [InlineData(null, "01/01/2023 12:00:00 AM", 10, "01/01/2022 12:00:00 AM", "01/01/2023 12:00:00 AM")]
        [InlineData("", "01/01/2023 12:00:00 AM", 10, "01/01/2022 12:00:00 AM", "01/01/2023 12:00:00 AM")]
        [InlineData("01/01/2022 12:00:00 AM", "01/01/2023 12:00:00 AM", 10, "01/01/2022 12:00:00 AM", "01/01/2023 12:00:00 AM")]
        [InlineData("01/01/2023 12:00:00 AM", null, 0, "01/01/2023 12:00:00 AM", "02/01/2022 04:40:00 PM")]
        [InlineData(null, "01/01/2003 12:00:00 AM", 0, "01/01/2022 12:00:00 AM", "01/01/2003 12:00:00 AM")]
        [InlineData("01/01/2002 12:00:00 AM", "01/01/2003 12:00:00 AM", 0, "01/01/2002 12:00:00 AM", "01/01/2003 12:00:00 AM")]
        [InlineData("01/01/2022 10:00:00 AM", "02/01/2022 12:00:00 AM", 40, "01/01/2022 10:00:00 AM", "02/01/2022 12:00:00 AM")]
        public void CalculateBillingBetweenTwoDates(string StartDateString,
                                                    string EndDateString, 
                                                    int amount,
                                                    string expectedBillStartDateString,
                                                    string expectedBillEndDateString
            )
        {
            DateTime? startDate = null;
            DateTime? endDate = null;
            if (!string.IsNullOrEmpty(StartDateString)) startDate = DateTime.Parse(StartDateString);
            if (!string.IsNullOrEmpty(EndDateString)) endDate = DateTime.Parse(EndDateString);
            DateTime expectedBillStartDate = DateTime.Parse(expectedBillStartDateString);
            DateTime expectedBillEndDate = DateTime.Parse(expectedBillEndDateString);
            _smartMeterToPricePlanAccounts = new Dictionary<string, Supplier>()
            {
                { SMART_METER_ID,Supplier.DrEvilsDarkEnergy}
            };
            _calculatorService= new CalculatorService(_meterReadingService, _meterAssociatedReadings, _pricePlans); 
            _billingService = new BillingService(_meterAssociatedReadings, _smartMeterToPricePlanAccounts, _pricePlans,_calculatorService);
            Bill bill = _billingService.GenerateBill(SMART_METER_ID, startDate, endDate);
            Assert.Equal(amount, Math.Round(bill.Amount));
            Assert.Equal(expectedBillStartDate, bill.StartDate);
            Assert.Equal(expectedBillEndDate, bill.EndDate);
        }


    }
}
