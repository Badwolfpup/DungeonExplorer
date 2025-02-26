using DungeonMaster.Equipment;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DungeonMaster.Classes
{
    public class Warrior : BaseClass
    {
        #region properties


        #endregion

        public Warrior(string name, string classname) : base(name, classname)
        {
            GenerateStartingEquipment();
            GenerateStartingItems();
        }

        public override void RollStats()
        {
            Random rnd = new Random();
            
            BaseStrength = rnd.Next(10, 21);
            BaseDexterity = rnd.Next(7, 16);
            BaseIntelligence = rnd.Next(5, 14);
            CalculateMaxHealth();
            Health = MaxHealth;
            CalculateMaxMana();
            Mana = MaxMana;
            CalculateLuck();
            CalculateCrit();
            Level = 1;
            Experience = 0;
            Gold = 0;
            Armor = (int)(Strength * 0.7 + Dexterity * 0.3) + 100;
            Resistance = (int)(Intelligence * 0.7 + Dexterity * 0.3) + 100;
            SkillList = PrintSkillList();
        }

        public override string LevelUp()
        {
            Random rnd = new Random();
            Level++;
            Experience = 0;
            int strgain = rnd.Next(3, 8);
            int dexgain = rnd.Next(2, 6);
            int intgain = rnd.Next(1, 4);
            BaseStrength += strgain;
            BaseDexterity += dexgain;
            BaseIntelligence += intgain;
            int skilluppchance = rnd.Next(100);
            int skillup = rnd.Next(4);
            if (skilluppchance > 19) _skillevels[skillup]++;
            return $"You have leveled up! You are now level {Level}! \n" +
                $"You gained {strgain} strength, {dexgain} dexterity and {intgain} intelligence \n" +
                $"{(skilluppchance < 49 ? "You didn't gain any skillevels this time" : $"You leveled up your {SkillList[skillup].Key} skill to level {_skillevels[skillup]}")}";
        }

        public override List<KeyValuePair<string, Action>> PrintSkillList()
        {
            return new List<KeyValuePair<string, Action>>()
            {
                new KeyValuePair<string, Action>("1. Bash", () => { Skill1(); }),
                new KeyValuePair<string, Action>("2. Charge", () => { Skill2(); }),
                new KeyValuePair<string, Action>("3. Slam", () => { Skill3(); }),
                new KeyValuePair<string, Action>("4. Roar", () => { Skill4(); }),
            };
        }

        public void GenerateStartingEquipment()
        {
            Equipment = new List<IEquipment>()
            {
                new Weapon("Wooden sword"),
                new Head("Paper helmet"),
                new Chest("Grass breastplate")
            };
            
        }


        public override void GenerateStartingItems()
        {
            Bag.Add(new Items.RandomizedItem("Health"));
            Bag.Add(new Items.RandomizedItem("Health"));
            Bag.Add(new Items.RandomizedItem("Health"));
            Bag.Add(new Items.RandomizedItem("Mana"));
        }






        public override double Attack()
        {
            Random rnd = new Random();
            double x = rnd.Next(30);
            double modifier = 1.0 + (x / 100);
            double damage = (5 + Strength / 2) * modifier;
            int lifesteal = (int)Math.Round(damage * 0.1);
            Health = Health + lifesteal <= MaxHealth ? Health + lifesteal : MaxHealth;
            return damage;
        }

        public override string Skill1()
        {
            Console.WriteLine("You bash the enemy with the pommel of your weapon");
            return "You bash the enemy with the pommel of your weapon";
        }

        public override string Skill2()
        {
            return "You charge at the enemy";
        }

        public override string Skill3()
        {
            return "You slam the enemy with your shield";
        }

        public override string Skill4()
        {
            return "You let out a mighty roar, temporarily increasing your strength";
        }
    }
}
