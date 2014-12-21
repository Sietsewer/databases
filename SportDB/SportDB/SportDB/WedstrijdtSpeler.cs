using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportDB
{
    public class WedstrijdtSpeler :Lid
    {
        public int Bondsnummer { get; set; }
        //public Team Team { get; set; }
    }
}
