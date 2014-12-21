using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportDB
{
    public class Training
    {
        public int TrainingID { get; set; }
        public int dagVanWeek { get; set; }
        public int Duur { get; set; }
        public Zaal zaal { get; set; }
    }
}
