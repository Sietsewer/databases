using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConsole_01
{
    class ClubDbContext : DbContext
    {
        public DbSet<Leden> Leden { get; set; }
        public DbSet<Teams> Teams { get; set; }
        public DbSet<Boetes> Boetes { get; set; }
        public DbSet<Wedstrijden> Wedstrijden { get; set; }
        public DbSet<Training> Training { get; set; }
        public DbSet<Zalen> Zalen { get; set; }

    }
}