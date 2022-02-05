using JOIEnergy.Domain;
using Microsoft.EntityFrameworkCore;

namespace JOIEnergy.Context
{
    public class MeterReadingContext: DbContext
    {
        public DbSet<PeakTimeMultiplier> PeakTimeMultipliers { get; set; }

        public MeterReadingContext(DbContextOptions<MeterReadingContext> options) : base(options)
        { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlite("Data Source=MeterReading.Db");
        }
    }
}