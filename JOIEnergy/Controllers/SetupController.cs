using JOIEnergy.Domain;
using JOIEnergy.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JOIEnergy.Controllers
{
    [Route("setup")]
    public class SetupController : Controller
    {
        private IPeakTimeMultiplierRepository _peakTimeMultiplierRepository;
        public SetupController(IPeakTimeMultiplierRepository peakTimeMultiplierRepository)
        {
            _peakTimeMultiplierRepository = peakTimeMultiplierRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("setup")]
        public void Setup()
        {
            
            PeakTimeMultiplier peakTimeMultiplier = new PeakTimeMultiplier()
            {
                DayOfWeek = DayOfWeek.Sunday,
                Multiplier = 2m
            };
            _peakTimeMultiplierRepository.Add(peakTimeMultiplier);

            peakTimeMultiplier = new PeakTimeMultiplier()
            {
                DayOfWeek = DayOfWeek.Monday,
                Multiplier = 4m
            };
            _peakTimeMultiplierRepository.Add(peakTimeMultiplier);
        }
    }
}
