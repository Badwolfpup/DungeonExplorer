using DungeonMaster.Equipment;
using DungeonMaster.Items;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMaster.Classes
{
    public abstract class BaseClass
    {


        public List<IEquipment> Equipment { get; set; }
        protected List<KeyValuePair<string, Action>> SkillList = new List<KeyValuePair<string, Action>>();
        protected List<RandomizedItem> Bag = new List<RandomizedItem>();
        protected int[] _skillevels = new int[4];
        protected int _basetrength;
        protected int _basedexterity;
        protected int _baseintelligence;

        protected BaseClass(string name, string className)
        {
            Name = name;
            ClassName = className;
        }

        public string Name { get; set; }
        public string ClassName { get; set; }
        public int MaxHealth { get; set; }
        public int Health { get; set; }
        public int MaxMana { get; set; }
        public int Mana { get; set; }
        public int BaseStrength
        {
            get => _basetrength;
            set
            {
                _basetrength = value;
                CalculateAttributes();
            }
        }
        public int BaseDexterity
        {
            get => _basedexterity;
            set
            {
                _basedexterity = value;
                CalculateAttributes();
            }
        }
        public int BaseIntelligence
        {
            get => _baseintelligence;
            set
            {
                _baseintelligence = value;
                CalculateAttributes();
            }
        }
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Intelligence { get; set; }
        public int Crit { get; set; }
        public int Luck { get; set; }
        public int Level { get; set; }
        public int Experience { get; set; }
        public int Gold { get; set; }
        public int Armor { get; set; }
        public int Resistance { get; set; }
        public double PhysicalDamageResist { get; set; }
        public double MagicalDamageResist { get; set; }
        public double MaxHealthModifier { get; set; } 
        public double MaxManaModifier { get; set; } 

        public void AddStartingItems()
        {
            
        }
        public void CalculateAttributes()
        {
            int str = BaseStrength;
            int dex = BaseDexterity;
            int intel = BaseIntelligence;
            if (Equipment != null)
            {
                foreach (IEquipment item in Equipment)
                {
                    str += item.Strength;
                    dex += item.Dexterity;
                    intel += item.Intelligence;
                }
            }
            Strength = str;
            Dexterity = dex;
            Intelligence = intel;
        }
        public void CalculateMaxHealth()
        {
            MaxHealth = 50 + Strength * 10;
        }
        public void CalculateMaxMana()
        {
            MaxMana = 50 + Intelligence * 10;
        }
        public void CalculateCrit()
        {
            Crit = 10 + (int)(Dexterity * 0.5);
        }
        public void CalculateLuck()
        {
            Luck = 10 + (int)(Dexterity * 0.2) + (int)(Intelligence * 0.2);
        }
        public void CalculatePhysicalDamageResist()
        {
            PhysicalDamageResist = Armor < 800 ? Armor / 1000 : 0.8;
        }
        public void CalculateMagicalDamageResist()
        {
            MagicalDamageResist = Resistance < 800 ? Resistance / 1000 : 0.8;
        }

        public void PrintStats()
        {
            Console.WriteLine($"Name: {Name}\n" +
                $"Class: {ClassName}\n " +
                $"Level: {Level}\n" +
                $"Experience: {Experience}\n" +
                $"Gold: {Gold}\n" +
                $"Health: {Health}/{MaxHealth}\n" +
                $"Mana: {Mana}/{MaxMana}\n" +
                $"Strength: {Strength}\n" +
                $"Dexterity: {Dexterity}\n" +
                $"Intelligence: {Intelligence}\n" +
                $"Crit: {Crit}\nLuck: {Luck}\n" +
                $"Armor: {Armor}\n" +
                $"Resistance: {Resistance}\n" +
                $"Physical Damage Resist: {PhysicalDamageResist}\n" +
                $"Magical Damage Resist: {MagicalDamageResist}");
        }
        public void PrintRolledStats()
        {
            Console.WriteLine($"Health: {Health}/{MaxHealth}\n" +
                $"Mana: {Mana}/{MaxMana}\n" +
                $"Strength: (10-20) {BaseStrength}\n" +
                $"Dexterity: (7-15) {BaseDexterity}\n" +
                $"Intelligence: (5-13) {BaseIntelligence}\n");

        }
        public void PrintEquipment()
        {
            Console.WriteLine("You currently have these items equipped:");
            foreach (IEquipment item in Equipment)
            {
                Console.WriteLine(item.ToString());
            }

        }

        public void PrintBag()
        {
            Console.WriteLine("You currently have these items in your bag:");
            foreach (RandomizedItem item in Bag)
            {
                Console.WriteLine(item.ToString());
            }
        }


        public abstract void RollStats();
        public abstract void GenerateStartingItems();
        public abstract string LevelUp();
        public abstract List<KeyValuePair<string, Action>> PrintSkillList();
        public abstract string Skill1();
        public abstract string Skill2();
        public abstract string Skill3();
        public abstract string Skill4();
        public abstract double Attack();

        public List<KeyValuePair<string, Action>> PrintUseItems()
        {
            List<KeyValuePair<string, Action>> bag = new List<KeyValuePair<string, Action>>();
            foreach (var item in Bag)
            {
                bag.Add(new KeyValuePair<string, Action>($"{bag.Count +1}. {item.Name}", () => { item.PrintDescription(); }));
            }
            return bag;
        }
    }
}
