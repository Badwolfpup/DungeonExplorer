using DungeonMaster.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMaster.Other
{
    public class Room
    {
        public IEvent CurrentEvent { get; set; }

        public string Icon => GetIcon();
        public bool IsFirstRoom { get; set; }
        public bool IsSolved { get; set; }

        private string GetIcon()
        {
            string icon = CurrentEvent.Type switch
            {
                "Altar" => "A",  // Torii (Shinto shrine gate)
                "Battle" => "X",  // Crossed swords
                "Boss" => "B",  // Oni face
                "Merchant" => "M",  // Shopping cart
                "prisoner" => "P",  // Chains
                "Eiddle" => "?",  // Question mark
                "Stairs" => "^",  // Upwards arrow
                "Teleport" => "T",  // Lightning bolt
                "Training" => "+",  // Flexed biceps
                "Treasure" => "*",  // Money bag
                _ => "?",   // Unknown event
            };
            return icon;
        }

        public Room() 
        {
            Random rnd = new Random();
            int random = rnd.Next(100);
            CurrentEvent = random switch
            {
                >= 50 => new Treasure(),
                >= 0 => new Treasure(),
            };
        }
    }
}
