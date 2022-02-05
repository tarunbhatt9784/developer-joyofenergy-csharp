using System;
using System.Collections.Generic;
using JOIEnergy.Services;
using JOIEnergy.Domain;
using Xunit;
using System.Linq;

namespace JOIEnergy.Tests
{
    public class MeterReadingServiceTest
    {
        private static string SMART_METER_ID = "smart-meter-id";

        private MeterReadingService meterReadingService;

        public MeterReadingServiceTest()
        {
            meterReadingService = new MeterReadingService(new Dictionary<string, List<ElectricityReading>>());

            meterReadingService.StoreReadings(SMART_METER_ID, new List<ElectricityReading>() {
                new ElectricityReading() { Time = DateTime.Now.AddMinutes(-30), Reading = 35m },
                new ElectricityReading() { Time = DateTime.Now.AddMinutes(-15), Reading = 30m }
            });
        }

        [Fact]
        public void GivenMeterIdThatDoesNotExistShouldReturnNull() {
            Assert.Empty(meterReadingService.GetReadings("unknown-id"));
        }

        [Fact]
        public void GivenMeterReadingThatExistsShouldReturnMeterReadings()
        {
            meterReadingService.StoreReadings(SMART_METER_ID, new List<ElectricityReading>() {
                new ElectricityReading() { Time = DateTime.Now, Reading = 25m }
            });

            var electricityReadings = meterReadingService.GetReadings(SMART_METER_ID);

            Assert.Equal(3, electricityReadings.Count);
        }

        [Fact]
        public void AddNewMeterId()
        {
            var meterReadingId = "meterid-100";
            List<ElectricityReading> readings = new List<ElectricityReading>();
            meterReadingService.StoreReadings(meterReadingId, readings);
            Assert.Equal(0, meterReadingService.GetReadings(meterReadingId).Count);
        }

        [Fact]
        public void AddNewMeterIdWithElectricityReading()
        {
            var meterReadingId = "meterid-100";
            List<ElectricityReading> readings = new List<ElectricityReading>();
            ElectricityReading reading = new ElectricityReading();
            reading.Reading = 2m;
            reading.Time = DateTime.UtcNow;
            readings.Add(reading);
            meterReadingService.StoreReadings(meterReadingId, readings);
            Assert.Equal(1, meterReadingService.GetReadings(meterReadingId).Count);
        }

        [Fact]
        public void AddNewMeterIdWithElectricityReadingWithNoDate()
        {
            var meterReadingId = "meterid-9999";
            List<ElectricityReading> readings = new List<ElectricityReading>();
            ElectricityReading reading = new ElectricityReading();
            reading.Reading = 2m;
            readings.Add(reading);
            meterReadingService.StoreReadings(meterReadingId, readings);
            var meterReading = meterReadingService.GetReadings(meterReadingId);
            Assert.Equal(1, meterReading.Count);
            Assert.Equal((DateTime.UtcNow - meterReading.First().Time).Days, 0);
        }

        [Fact]
        public void DuplicateMeterReadingShouldDeleteOldReading()
        {
            var readingsForMeterId = meterReadingService.GetReadings(SMART_METER_ID);
            meterReadingService.StoreReadings(SMART_METER_ID, new List<ElectricityReading>()
            {
                new ElectricityReading()
                {
                    Reading=2m,
                    Time=readingsForMeterId.First().Time
                }
            });
            var updatedReadings = meterReadingService.GetReadings(SMART_METER_ID)
                                                     .Where(r=>r.Time.CompareTo(readingsForMeterId.First().Time)==0)
                                                     .OrderBy(r => r.Time);
            Assert.Null(updatedReadings.Last().DeletedAt);
            Assert.Equal(false, updatedReadings.SkipLast(1).Any(r => r.DeletedAt == null));
        }

    }
}
