﻿using DungeonMaster.Equipment;
using DungeonMaster.Items;
using DungeonMaster.Skills;
using DungeonMaster.Skills.Melee;
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
        protected BaseClass(string name, string className)
        {
            Name = name;
            ClassName = className;
        }
        #region Properties
        public List<IEquipment> Equipment { get; set; }
        public List<KeyValuePair<string, Action>> SkillList = new List<KeyValuePair<string, Action>>();
        public List<RandomizedItem> Bag = new List<RandomizedItem>();
        public List<Action> Effects = new List<Action>();
        public abstract List<BaseSkill> Skills { get; set; }
        protected int[] _skillevels = new int[4];

        public string Name { get; set; }
        public string ClassName { get; set; }
        public int Health { get; set; }
        public int Mana { get; set; }
        public int BaseStrength { get; set; }
        public int BaseDexterity { get; set; }
        public int BaseIntelligence { get; set; }
        public int Level { get; set; }
        public int Experience { get; set; }
        public int Gold { get; set; }
        public abstract double DamageResist { get;}
        public double MaxHealthModifier { get; set; } = 1.0;
        public double MaxManaModifier { get; set; } = 1.0;
        public double StrModifier { get; set; } = 1.0;
        public double DexModifier { get; set; } = 1.0;
        public double IntrModifier { get; set; } = 1.0;
        //public abstract double ArmorModifier { get; set; }
        public double ResModifier { get; set; } = 1.0;
        public double CritModifier { get; set; } = 1.0;
        public double LuckModifier { get; set; } = 1.0;
        public double DamageDoneModifier { get; set; } = 1.0;
        public double DamageTakenModifier { get; set; } = 1.0;
        public bool IsUsingAbility { get; set; }
        #endregion

        public void AddStartingItems()
        {
            
        }

        public int Strength => (int)((BaseStrength + Equipment.Sum(x => x.Strength)) * StrModifier);
        public int Dexterity => (int)((BaseDexterity + Equipment.Sum(x => x.Dexterity)) * DexModifier);
        public int Intelligence => (int)((BaseIntelligence + Equipment.Sum(x => x.Intelligence)) * IntrModifier);
        
        public int Crit => (int)((10 + Dexterity * 0.5) * CritModifier);
        public int MaxHealth => (int)((50 + Strength * 10) * MaxHealthModifier);
        public int MaxMana => (int)((50 + Intelligence * 10) * MaxManaModifier);
        public int Luck => (int)((10 + Dexterity * 0.2 + Intelligence * 0.2) * LuckModifier);

        public List<string> PrintCharacter()
        {
            return new List<string>
            {
                $" Name: {Name}",
                $" Class: {ClassName}",
                $" Level: {Level}",
                $" XP: {Experience}" +$" Gold: {Gold}",
                $" Health: {Health}/{MaxHealth}",
                $" Mana: {Mana}/{MaxMana}"
            };
        }

        public List<string> PrintMonster()
        {
            return new List<string>
            {
                $"Name: {Name}",
                //$"Class: {ClassName}",
                $"Level: {Level}",
                $"Health: {Health}/{MaxHealth}",
                $"Mana: {Mana}/{MaxMana}"
            };
        }

        public abstract List<string> PrintStats();


        public List<string> PrintRolledStats()
        {
             return new List<string> { 
                $"Health: {Health}/{MaxHealth}",
                $"Mana: {Mana}/{MaxMana}",
                $"Strength: (10-20) {BaseStrength}",
                $"Dexterity: (7-15) {BaseDexterity}",
                $"Intelligence: (5-13) {BaseIntelligence}" };

            //for (int i = 0; i < stats.Count; i++)
            //{
            //    Console.SetCursorPosition(31, i+1);
            //    Console.WriteLine(stats[i]);
            //}

        }
        public List<string> PrintEquipment()
        {
            List<string> items = new List<string>();
            Equipment.ForEach(x => items.Add(x.ToString()));
            return items;

        }

        public List<string> PrintBag()
        {
            List<string> bags = new List<string>();
            Bag.ForEach(x => bags.Add(x.ToString()));
            return bags;
        }

        public abstract void RollStats();
        public abstract void GenerateStartingItems();
        public abstract void LevelUp();
        public abstract List<KeyValuePair<string, Action>> PrintSkillList();
        
        public abstract void Attack(BaseClass attacker);
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
