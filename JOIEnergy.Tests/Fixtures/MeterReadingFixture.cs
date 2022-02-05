using JOIEnergy.Context;
using JOIEnergy.Repository;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace JOIEnergy.Tests.Fixtures
{
    public class MeterReadingFixture: IDisposable
    {
        public PeakTimeMultiplierRespository peakTimeMultiplierRespository;
        private MeterReadingContext _context;
        public MeterReadingFixture()
        {
            var connectionBuilder = new SqliteConnectionStringBuilder { DataSource = ":memory:" };
            var connection = new SqliteConnection(connectionBuilder.ToString());
            var options = new DbContextOptionsBuilder<MeterReadingContext>()
                            .UseSqlite(connection)
                            .Options;
            _context = new MeterReadingContext(options);
            _context.Database.OpenConnection();
            _context.Database.EnsureCreated();

            //Act
            peakTimeMultiplierRespository = new PeakTimeMultiplierRespository(_context);
        }

        public void Dispose()
        {
            _context.Dispose();
            peakTimeMultiplierRespository = null;
        }
    }
}
