using System;
using System.Collections.Generic;
using System.Linq;
using JOIEnergy.Domain;
using JOIEnergy.Enums;

namespace JOIEnergy.Services
{
    public class MeterReadingService : IMeterReadingService
    {
        public Dictionary<string, List<ElectricityReading>> MeterAssociatedReadings { get; set; }
        public MeterReadingService(Dictionary<string, List<ElectricityReading>> meterAssociatedReadings)
        {
            MeterAssociatedReadings = meterAssociatedReadings;
        }

        public List<ElectricityReading> GetReadings(string smartMeterId, ReadingStatus status=ReadingStatus.Eligible) {
            if (MeterAssociatedReadings.ContainsKey(smartMeterId)) {
                switch (status)
                {
                    case ReadingStatus.Eligible:
                        return MeterAssociatedReadings[smartMeterId].Where(r => r.DeletedAt == null).ToList();
                    case ReadingStatus.All:
                        return MeterAssociatedReadings[smartMeterId];
                }
            }
            return new List<ElectricityReading>();
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
