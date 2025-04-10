﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMaster.Equipment
{
    public class Weapon : IEquipment
    {
        public string Name { get; set; }
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Intelligence { get; set; }
        public bool HasEffect { get; set; }

        public Weapon(string name)
        {
            Strength = 5;
            Dexterity = 5;
            Intelligence = 5;
            Name = name;
        }

        public Weapon(int strength, int dexterity, int intelligence, string name)
        {
            Strength = strength;
            Dexterity = dexterity;
            Intelligence = intelligence;
            Name = name;
        }

        public Weapon(string name, int strength, int dexterity, int intelligence, int factor)
        {
            Random rnd = new Random();
            Strength = (int)Math.Round((strength * (rnd.Next(1, factor * 2) / 10.0 + 1)));
            Dexterity = (int)Math.Round((dexterity * (rnd.Next(1, factor * 2) / 10.0 + 1)));
            Intelligence = (int)Math.Round((intelligence * (rnd.Next(1, factor * 2) / 10.0 + 1)));
            Name = name;
        }

        public void Effect()
        {
            
        }

        public override string ToString() => Name;
    }
}
