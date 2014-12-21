using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace DatabaseConsole_01
{
    public class Wedstrijden
    {
        public Wedstrijden()
        {
            boetes = new List<Boetes>();
        }
        [Key]
        public int teamID { get; set; }

        public int wedstrijdnummer { get; set; }

        public string wedstrijddatum { get; set; }

        public Leden scheids { get; set; }

        public string tegenstander { get; set; }

        public Zalen wedstrijdzaal { get; set; }

        public virtual List<Boetes> boetes { get; private set; }

        public Teams teamcode { get; set; }
    }
}
