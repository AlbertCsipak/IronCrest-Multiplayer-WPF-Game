using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI_20212022_Z6O9JF.Models
{
    public interface IGameItem
    {
        public int OwnerId { get; set; }
        public int[] Position { get; set; }
        public bool CanMove { get; set; }
        public void Move(int[] pos);
    }
}
