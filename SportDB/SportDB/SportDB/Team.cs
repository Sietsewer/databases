using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace SportDB
{
    public class Team
    {
        public Team()
        {
            Spelers = new List<Lid>();
            Trainingen = new List<Training>();
        }
        public int TeamID { get; set; }
        public string TeamCode { get; set; }
        public Lid Trainer { get; set; }
        public virtual List<Lid> Spelers { get; private set; }
        public virtual List<Training> Trainingen { get; private set; }

        public string LeeftijdsKlasse { get; set; }

        public string Sexe { get; set; }
    }
}
