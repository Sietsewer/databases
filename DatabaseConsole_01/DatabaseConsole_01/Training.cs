using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace DatabaseConsole_01
{
    public class Training
    {
        [Key]
        public int trainingID { get; set; }

        public int traindag { get; set; }

        public int traintijd { get; set; }

        public int trainlengte { get; set; }

        public Zalen trainzaal { get; set; }
    }
}
