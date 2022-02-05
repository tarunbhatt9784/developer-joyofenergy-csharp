using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using JOIEnergy.Enums;

namespace JOIEnergy.Domain
{
    public class PricePlan
    {
        public Supplier EnergySupplier { get; set; }
        public decimal UnitRate { get; set; }
        public IList<PeakTimeMultiplier> PeakTimeMultiplier { get; set;}

        public decimal GetPrice(DateTime datetime) {
            var multiplier = PeakTimeMultiplier.FirstOrDefault(m => m.DayOfWeek == datetime.DayOfWeek);

            if (multiplier?.Multiplier != null) {
                return multiplier.Multiplier * UnitRate;
            } else {
                return UnitRate;
            }
        }
    }

    public class PeakTimeMultiplier
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PeakTimeMultiplierId { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public decimal Multiplier { get; set; }
    }
}
