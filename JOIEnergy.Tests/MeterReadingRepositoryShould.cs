using JOIEnergy.Context;
using JOIEnergy.Domain;
using JOIEnergy.Repository;
using JOIEnergy.Tests.Fixtures;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace JOIEnergy.Tests
{
    public class PeakTimeMultiplierRepositoryShould : IClassFixture<MeterReadingFixture>
    {
        private MeterReadingFixture _fixture;
        private IPeakTimeMultiplierRepository _peakTimeMultiplierRepository;

        public PeakTimeMultiplierRepositoryShould(MeterReadingFixture meterReadingFixture)
        {
            _fixture = meterReadingFixture;
            _peakTimeMultiplierRepository = _fixture.peakTimeMultiplierRespository;
        }
        
        [Theory]
        [InlineData(DayOfWeek.Sunday, 2)]
        [InlineData(DayOfWeek.Monday, 4)]
        public void AddPeakTimeMultipliers(DayOfWeek day, decimal multiplier)
        {
            //Arrange
            int count = _peakTimeMultiplierRepository.GetAll().Count;
            PeakTimeMultiplier peakTimeMultiplier = new PeakTimeMultiplier()
            {
                DayOfWeek = DayOfWeek.Sunday,
                Multiplier = multiplier
            };
            //Act
            _peakTimeMultiplierRepository.Add(peakTimeMultiplier);
            //Assert
            Assert.Equal(count+1, _peakTimeMultiplierRepository.GetAll().Count);
        }
    }
}
