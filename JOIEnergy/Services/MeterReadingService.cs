using System;
using System.Collections.Generic;
using System.Linq;
using JOIEnergy.Domain;

namespace JOIEnergy.Services
{
    public class MeterReadingService : IMeterReadingService
    {
        public Dictionary<string, List<ElectricityReading>> MeterAssociatedReadings { get; set; }
        public MeterReadingService(Dictionary<string, List<ElectricityReading>> meterAssociatedReadings)
        {
            MeterAssociatedReadings = meterAssociatedReadings;
        }

        public List<ElectricityReading> GetReadings(string smartMeterId) {
            if (MeterAssociatedReadings.ContainsKey(smartMeterId)) {
                return MeterAssociatedReadings[smartMeterId];
            }
            return new List<ElectricityReading>();
        }

        public List<ElectricityReading> GetEligibleReadings(string smartMeterId)
        {
            return GetReadings(smartMeterId).Where(r => r.DeletedAt == null).ToList();
        }

        public void StoreReadings(string smartMeterId, List<ElectricityReading> electricityReadings) {
            if (!MeterAssociatedReadings.ContainsKey(smartMeterId)) {
                MeterAssociatedReadings.Add(smartMeterId, new List<ElectricityReading>());
            }
            electricityReadings.ForEach(
                newReading=>
                    {
                        MeterAssociatedReadings[smartMeterId]
                            .Where(r => r.Time.CompareTo(newReading.Time) == 0)
                            .ToList()
                            .ForEach(reading => reading.DeletedAt = DateTime.UtcNow);
                        MeterAssociatedReadings[smartMeterId].Add(newReading);
                    }
                );
            //electricityReadings.ForEach(electricityReading => MeterAssociatedReadings[smartMeterId].Add(electricityReading));
        }
    }
}
