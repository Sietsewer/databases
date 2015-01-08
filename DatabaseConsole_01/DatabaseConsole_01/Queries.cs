using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConsole_01
{
    static class Queries
    {
        public static List<Leden> scheidsrechtersBijZaal(Zalen zaal)
        {
            return Program.context.Wedstrijden.Where(b => b.wedstrijdzaal.trainzaal == zaal.trainzaal).Select(c => c.scheids).ToList();
            //return null;
        }
        public static List<Leden> ouderDanToenLid(int l)
        {
            return Program.context.Leden.Where(b => (b.jaarlid - b.gebjaar) >= l).ToList();
        }
        public static Leden maxBoetes()
        {
            return Program.context.Boetes.OrderByDescending(b => b.bedrag_boete).First().speler;
        }
        public static Leden maxBoetesCount()
        {
            throw new NotImplementedException();
            return null;
            //return Program.context.Boetes.GroupBy(x => new { x.speler, x.boeteID });
        }
    }
}
