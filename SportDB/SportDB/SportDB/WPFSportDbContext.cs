using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportDB
{
    public class WPFSportDbContext:DbContext
    {
        public DbSet<Lid> Leden { get; set; }
        public DbSet<WedstrijdtSpeler> WedstrijdtSpelers { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Training> Trainingen { get; set; }
        public DbSet<Zaal> zalen { get; set; }
        public DbSet<Wedstrijd> Wedstrijden { get; set; }
        public DbSet<Boete> Boetes { get; set; }
        
    }
}
