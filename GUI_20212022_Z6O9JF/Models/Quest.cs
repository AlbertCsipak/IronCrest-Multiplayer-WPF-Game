using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI_20212022_Z6O9JF.Models
{
    public class Quest
    {
        public string Name { get; set; }
        public bool Done { get; set; }
        public Quest(int crit)
        {
         
        }
        public bool QuestDone(int crit) {
            if (crit>100)
            {
                return true;
            }
            return false;
        }
    }
}
