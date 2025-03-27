using DungeonMaster.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMaster.FormatClass
{
    public static class TextColor
    {
        public static ConsoleColor HealthMana(BaseClass chosen)
        {
            return (chosen.Health/chosen.MaxHealth) switch
            {
                >= 75 => ConsoleColor.Green,
                >= 25 => ConsoleColor.Yellow,
                _ => ConsoleColor.Red
            };

        }
    }
}
