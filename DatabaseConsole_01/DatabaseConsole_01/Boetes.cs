using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace DatabaseConsole_01
{
    public class Boetes
    {
        [Key]
        public int boeteID { get; set; }

        public int bedrag_boete { get; set; }

        public string toelichting { get; set; }

        public Leden speler { get; set; }

        public Wedstrijden wedstrijd { get; set; }
    }
}
