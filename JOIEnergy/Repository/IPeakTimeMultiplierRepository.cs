using JOIEnergy.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace JOIEnergy.Repository
{
    public interface IPeakTimeMultiplierRepository
    {
        void Add(PeakTimeMultiplier multiplier);
        IList<PeakTimeMultiplier> GetAll();
    }
}
