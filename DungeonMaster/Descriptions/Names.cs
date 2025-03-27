using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMaster.Descriptions
{
    public static class Names
    {
        static private readonly List<string> FirstNames = new List<string>
        {
            "Arthur", "William", "Edward", "Henry", "Eleanor",
            "Margaret", "Isabella", "Catherine", "Eldric", "Thalindra",
            "Garrick", "Odin", "Freya", "Thor", "Amina",
            "Kofi", "Mei", "Sakura", "Tala", "Nakoa",
            "Alaric", "Branwen", "Cedric", "Drusilla", "Elowen",
            "Finnian", "Gwendolyn", "Hadrian", "Isadora", "Jareth",
            "Kael", "Liora", "Magnus", "Niamh", "Orion",
            "Persephone", "Quinlan", "Rhiannon", "Silas", "Talia",
            "Uther", "Vespera", "Wystan", "Xanthe", "Yseult",
            "Zephyr", "Aiden", "Brienne", "Cillian", "Dara"
        };
        static private readonly List<string> LastNames = new List<string>
        {
            "Smith", "Baker", "Fletcher", "Cooper", "Miller",
            "Carpenter", "Hunter", "Farmer", "Weaver", "Taylor",
            "Hill", "Woods", "Rivers", "Fields", "Stone",
            "Brook", "Meadows", "Lake", "Forest", "Cliff",
            "Silverthorn", "Goldcrest", "Ironfist", "Stonehelm", "Brightblade",
            "Darkwood", "Ravenclaw", "Wolfhart", "Eagleeye", "Lionmane",
            "Shadowcaster", "Dragonheart", "Stormbringer", "Fireforge", "Ironwood",
            "Nightshade", "Moonshadow", "Starfall", "Windrider", "Frostbane",
            "Spellweaver", "Bladeborn", "Runekeeper", "Beastmaster", "Shadowalker",
            "Lightbringer", "Darkbane", "Soulrender", "Voidwalker", "Dreamweaver"
        };

        public static string RandomName()
        {
            Random rand = new Random();
            string firstName = FirstNames[rand.Next(FirstNames.Count)];
            string lastName = LastNames[rand.Next(LastNames.Count)];
            return firstName + " " + lastName;
        }
    }


}
