using System.Collections.Generic;
using JOIEnergy.Domain;
using JOIEnergy.Enums;

namespace JOIEnergy.Services
{
    public interface IMeterReadingService
    {
        List<ElectricityReading> GetReadings(string smartMeterId, ReadingStatus readingStatus = ReadingStatus.All );
        void StoreReadings(string smartMeterId, List<ElectricityReading> electricityReadings);
    }
}