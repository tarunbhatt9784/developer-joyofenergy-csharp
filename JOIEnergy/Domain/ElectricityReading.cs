using System;
namespace JOIEnergy.Domain
{
    public class ElectricityReading
    {
        public DateTime Time
        {
            get
            {
                if (_readingTime.CompareTo(DateTime.MinValue) == 0)
                    return DateTime.UtcNow;
                else return _readingTime;
            }
            set
            {
                if (value.CompareTo(DateTime.MinValue) == 0)
                    _readingTime = DateTime.UtcNow;
                else _readingTime = value;
            }
        }

        private DateTime _readingTime { get; set; }
        public Decimal Reading { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
