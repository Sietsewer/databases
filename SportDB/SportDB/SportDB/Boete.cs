using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportDB
{
    public class Boete
    {
        public int BoeteID { get; set; }
        public int Bedrag { get; set; }
        public string Toelichting { get; set; }
        public Lid Speler { get; set; }
        public Wedstrijd Wedstrijdt { get; set; }
        public string Verenging { get; set; }
    }
}
