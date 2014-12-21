using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace DatabaseConsole_01
{
    public class Teams
    {
        public Teams()
        {
            spelers = new List<Leden>();
            trainingen = new List<Training>();
        }
        [Key]
        public int teamID { get; set; }

        public string teamcode { get; set; }

        public Leden trainer { get; set; }

        public virtual List<Leden> spelers { get; private set; }

        public virtual List<Training> trainingen { get; private set; }

        public string sexe { get; set; }

        public string leeftijdsklasse { get; set; }
    }
}
