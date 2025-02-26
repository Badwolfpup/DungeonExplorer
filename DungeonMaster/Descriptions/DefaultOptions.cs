using DungeonMaster.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMaster.Descriptions
{
    public static class DefaultOptions
    {
        public static List<KeyValuePair<string, Action>> AddCharacterOptions(List<KeyValuePair<string, Action>> Options,BaseClass chosenclass)
        {
            Options.Add(new KeyValuePair<string, Action>($"{Options.Count + 1}. View character stats", () => { chosenclass.PrintStats(); }));
            Options.Add(new KeyValuePair<string, Action>($"{Options.Count + 1}. View equipment", () => { chosenclass.PrintEquipment(); }));
            Options.Add(new KeyValuePair<string, Action>($"{Options.Count + 1}. View items in bag", () => { chosenclass.PrintBag(); }));
            return Options;
        }
    }
}
