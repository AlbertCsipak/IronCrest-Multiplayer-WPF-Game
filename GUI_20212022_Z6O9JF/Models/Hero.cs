﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI_20212022_Z6O9JF.Models
{
    public enum HeroType { Viking, Crusader, Mongolian, Arabian }
    public class Hero
    {
        public string Name { get; set; }
        public int Damage { get; set; }
        public int[,] Position { get; set; }
        public HeroType HeroType { get; set; }
    }
}
