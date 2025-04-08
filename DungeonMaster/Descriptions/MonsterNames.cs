using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DungeonMaster.Descriptions
{
    public static class MonsterNames
    {
        private static Random _random = new Random();
        private static List<string> _monsternames = new List<string>()
        {
            "Goblin", "Orc", "Troll", "Dragon", "Skeleton", "Zombie", "Vampire", "Werewolf", "Ghost", "Lich", "Demon", "Gargoyle",
            "Golem", "Kraken", "Beholder", "Mimic", "Chimera", "Griffon", "Basilisk", "Harpy"
        };
        private static List<string> _monsteradjectives = new List<string>()
        {
            "Angry", "Hungry", "Vicious", "Sneaky", "Sly", "Cunning", "Savage", "Ferocious", "Bloodthirsty", "Ravenous", "Fierce",
            "Wild", "Cruel", "Brutal", "Merciless", "Vile", "Evil"
        };
        private static List<string> _monstersuffix = new List<string>()
        {
            "of Doom", "of the Abyss", "of Shadows", "of the Fallen", "of Eternal Night", "of the Phoenix", "of the Lich", "of the Wilds",
            "of Frost", "of Infernos", "of the Arcane", "of Corruption", "of the Void", "of the Storm", "of Reckoning", "of the Titans",
            "of the Forgotten", "of the Cursed One", "of the Dragon", "of the Undying"
        };

        public static string RandomMonsterName()
        {
            return _monsternames[_random.Next(_monsternames.Count)];

        }

        public static string RandomBossName()
        {
            return bossNames[_random.Next(bossNames.Count)];

        }

        public static string RandomMonsterFullName(string name)
        {
            string adjective = _monsteradjectives[_random.Next(_monsteradjectives.Count)];
            string suffix = _monstersuffix[_random.Next(_monstersuffix.Count)];
            return adjective + " " + name + " " + suffix;
        }

        private static List<string> bossNames = new List<string>
        {
            "Emperor Mateus",
            "Cloud of Darkness",
            "Zeromus",
            "Exdeath",
            "Kefka",
            "Sephiroth",
            "Dragonlord",
            "Hargon",
            "Baramos",
            "Necrosaro",
            "Lavos",
            "Magus",
            "Mana Beast",
            "Dark Force",
            "Neclord",
            "Tyr",
            "Dark Dragon",
            "Tiamat",
            "Orcus",
            "Demogorgon",
            "Vecna",
            "Strahd von Zarovich",
            "Acererak",
            "Lolth",
            "Asmodeus",
            "Atma Weapon",
            "Warmech",
            "Omega",
            "Shinryu"
        };
    }
}
