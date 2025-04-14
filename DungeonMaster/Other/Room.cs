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
                "Prisoner" => "P",  // Chains
                "Riddle" => "?",  // Question mark
                "Stairs" => "˅",  // Upwards arrow
                "Teleport" => "T",  // Lightning bolt
                "Training" => "+",  // Flexed biceps
                "Treasure" => "$",  // Money bag
                _ => "˄",   // Starting position
            };
            return icon;
        }

        public Room() 
        {
            Random rnd = new Random();
            int random = rnd.Next(100);
            //CurrentEvent = random switch
            //{
            //    >= 90 => new Altar(),
            //    >= 80 => new TrainingRoom(),
            //    >= 70 => new Merchant(),
            //    >= 60 => new Prisoner(),
            //    >= 50 => new Riddle(),
            //    >= 40 => new Treasure(),
            //    >= 30 => new Teleport(),
            //    >= 0 => new Battle(),

            //    _  => new Battle(),
            //};
            CurrentEvent = random switch
            {
                >= 0 => new Battle(),


                _ => new TrainingRoom(),
            };
        }
    }
}
