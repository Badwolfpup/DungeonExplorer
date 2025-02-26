using DungeonMaster.Equipment;
using DungeonMaster.Monsters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace DungeonMaster.Classes
{
    public class Monster
    {
        public string Name { get; set; }
        public string MonsterName { get; set; }
        public int MaxHealth { get; set; }
        public int Health { get; set; }
        public int MaxMana { get; set; }
        public int Mana { get; set; }
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Intelligence { get; set; }
        public int Crit { get; set; }
        public int Luck { get; set; }
        public int Level { get; set; }
        public int Armor { get; set; }
        public int Resistance { get; set; }
        public double PhysicalDamageResist { get; set; }
        public double MagicalDamageResist { get; set; }
        private double _modifier;
        public Monster(string name, string monsterName)
        {
            MonsterName = monsterName;
            Name = name;           
            SetModifier();
            RollStats();
            Armor = (int)(Strength * 0.7 + Dexterity * 0.3) + 100;
            Resistance = (int)(Intelligence * 0.7 + Dexterity * 0.3) + 100;
            CalculateMaxHealth();
            CalculateMaxMana();
            CalculateCrit();
            CalculateLuck();
            CalculatePhysicalDamageResist();
            CalculateMagicalDamageResist();

        }

        private void SetModifier()
        {
            switch (Name)
            {
                case "Goblin":
                    _modifier = 1.0;
                    break;
                case "Orc":
                    _modifier = 1.5;
                    break;
                case "Troll":
                    _modifier = 2.0;
                    break;
                case "Dragon":
                    _modifier = 3.0;
                    break;
                case "Skeleton":
                    _modifier = 1.2;
                    break;
                case "Zombie":
                    _modifier = 1.3;
                    break;
                case "Vampire":
                    _modifier = 2.5;
                    break;
                case "Werewolf":
                    _modifier = 2.2;
                    break;
                case "Ghost":
                    _modifier = 1.8;
                    break;
                case "Lich":
                    _modifier = 3.5;
                    break;
                case "Demon":
                    _modifier = 3.0;
                    break;
                case "Gargoyle":
                    _modifier = 2.1;
                    break;
                case "Golem":
                    _modifier = 2.8;
                    break;
                case "Kraken":
                    _modifier = 4.0;
                    break;
                case "Beholder":
                    _modifier = 3.7;
                    break;
                case "Mimic":
                    _modifier = 1.6;
                    break;
                case "Chimera":
                    _modifier = 2.9;
                    break;
                case "Griffon":
                    _modifier = 2.7;
                    break;
                case "Basilisk":
                    _modifier = 3.2;
                    break;
                case "Harpy":
                    _modifier = 1.9;
                    break;
                default:
                    _modifier = 1.0;
                    break;
            }

        }


        public void CalculateCrit()
        {
            Crit = 10 + (int)(Dexterity * 0.5);
        }

        public void CalculateLuck()
        {
            Luck = 10 + (int)(Dexterity * 0.2) + (int)(Intelligence * 0.2);
        }

        public void CalculateMaxHealth()
        {
            MaxHealth = 30 + Strength * 5;
            Health = MaxHealth;
        }

        public void CalculateMaxMana()
        {
            MaxMana = 50 + Intelligence * 10;
            Mana = MaxMana;
        }

        public void CalculatePhysicalDamageResist()
        {
            PhysicalDamageResist = Armor < 800 ? Armor / 1000.0 : 0.8;
        }

        public void CalculateMagicalDamageResist()
        {
            MagicalDamageResist = Resistance < 800 ? Resistance / 1000.0 : 0.8;
        }

        public void RollStats()
        {
            Random rnd = new Random();
            Strength = (int)(rnd.Next(5, 11) * _modifier);
            Dexterity = (int)(rnd.Next(5, 11) * _modifier);
            Intelligence = (int)(rnd.Next(5, 11) * _modifier);
        }

        public string Skill1()
        {
            throw new NotImplementedException();
        }

        public string Skill2()
        {
            throw new NotImplementedException();
        }

        public string Skill3()
        {
            throw new NotImplementedException();
        }

        public string Skill4()
        {
            throw new NotImplementedException();
        }

        public List<KeyValuePair<string, Action>> SkillList()
        {
            throw new NotImplementedException();
        }
    }
}