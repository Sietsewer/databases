using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportDB
{
   public class Wedstrijd
    {
       public Wedstrijd()
       {
           Boetes = new ObservableCollection<Boete>();
       }
       public int ID { get; set; }
       public int WedstrijdtNummer { get; set; }
       public Zaal GespeeldIn { get; set; }
       public Lid Scheidsrechter { get; set; }
       public Team Team { get; set; }
       public virtual ObservableCollection<Boete> Boetes { get; private set; }

       public string WedstrijdtDatum { get; set; }



       public string GespeeldTegen { get; set; }
    }
}
