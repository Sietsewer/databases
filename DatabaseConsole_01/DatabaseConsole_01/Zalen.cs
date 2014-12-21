using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace DatabaseConsole_01
{
    public class Zalen
    {
        [Key]
        public int zaalID { get; set; }

        public string zaaladres { get; set; }

        public string trainzaal { get; set; }

        public string zaaltel { get; set; }
    }
}
