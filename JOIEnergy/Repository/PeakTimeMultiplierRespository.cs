using JOIEnergy.Context;
using JOIEnergy.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JOIEnergy.Repository
{
    public class PeakTimeMultiplierRespository : IPeakTimeMultiplierRepository
    {
        private MeterReadingContext _context;
        public PeakTimeMultiplierRespository(MeterReadingContext context)
        {
            _context = context;
        }
        public void Add(PeakTimeMultiplier multiplier)
        {
            _context.Add(multiplier);
            _context.SaveChanges();
        }
        public IList<PeakTimeMultiplier> GetAll()
        {
            return _context.PeakTimeMultipliers.ToList();
        }
    }
}
