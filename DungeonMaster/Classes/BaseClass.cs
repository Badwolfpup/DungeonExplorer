using DungeonMaster.Descriptions;
using DungeonMaster.Equipment;
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
    /// <summary>
    /// The parentclass that both players and monster inherits from. Contains most of the properties and methods that are used.
    public abstract class BaseClass
    {
        protected BaseClass(string name, string className)
        {
            Name = name;
            ClassName = className;
        }
        #region Properties
        public List<IEquipment> Equipment { get; set; } //List of equipment that the entity has
        public List<KeyValuePair<string, Action>> SkillList = new List<KeyValuePair<string, Action>>(); //List of skills that the entity has
        public List<RandomizedItem> Bag = new List<RandomizedItem>(); //List of items that the entity has
        public List<Action> Effects = new List<Action>(); //List of effects that the entity is currently affected by
        public abstract List<BaseSkill> Skills { get; set; } //List of skills that the entity has
        protected int[] _skillevels = new int[4]; //Arrar of skill levels that the entity's skills are

        //Stats of the entity
        public string Name { get; set; }
        public string ClassName { get; set; }
        public int Health { get; set; }
        public int Mana { get; set; }
        public int BaseStrength { get; set; }
        public int BaseDexterity { get; set; }
        public int BaseIntelligence { get; set; }
        public int Level { get; set; }
        public int Experience { get; set; }
        public abstract double DamageResist { get;}

        //Modifiers for the stats
        public double MaxHealthModifier { get; set; } = 1.0;
        public double MaxManaModifier { get; set; } = 1.0;
        public double StrModifier { get; set; } = 1.0;
        public double DexModifier { get; set; } = 1.0;
        public double IntModifier { get; set; } = 1.0;
        public double AltarStrModifier { get; set; } = 1.0;
        public double AltarDexModifier { get; set; } = 1.0;
        public double AltarIntModifier { get; set; } = 1.0;
        public double ResModifier { get; set; } = 1.0;
        public double CritModifier { get; set; } = 1.0;
        public double AltarCritModifier { get; set; } = 1.0;
        public double DamageDoneModifier { get; set; } = 1.0;
        public double DamageTakenModifier { get; set; } = 1.0;
        public double AltarDamageDoneModifier { get; set; } = 1.0;
        public double AltarDamageTakenModifier { get; set; } = 1.0;

        public bool IsUsingAbility { get; set; } //Used to check if the entity is using an ability
        //public bool HasDebuffNextBattle { get; set; } //Used to check if the entity has a debuff that will be removed next battle
        #endregion

        public void AddStartingItems()
        {
            
        }

        public void ResetAltarBuffsDebuffs() //Resets the buffs and debuffs applied by the altar
        {
            AltarStrModifier = 1.0;
            AltarDexModifier = 1.0;
            AltarIntModifier = 1.0;
            AltarCritModifier = 1.0;
            AltarDamageDoneModifier = 1.0;
            AltarDamageTakenModifier = 1.0;
        }

        //Getters for the stats that are calculated from the e.g base stats and equipment
        public int Strength => (int)((BaseStrength + Equipment.Sum(x => x.Strength)) * StrModifier * AltarStrModifier) + 1000;
        public int Dexterity => (int)((BaseDexterity + Equipment.Sum(x => x.Dexterity)) * DexModifier * AltarDexModifier);
        public int Intelligence => (int)((BaseIntelligence + Equipment.Sum(x => x.Intelligence)) * IntModifier * AltarIntModifier);       
        public int Crit => (int)((10 + Dexterity * 0.5) * CritModifier * AltarCritModifier);
        public int MaxHealth => (int)((50 + Strength * 10) * MaxHealthModifier);
        public int MaxMana => (int)((50 + Intelligence * 10) * MaxManaModifier);

        //Abstract methods that are used to be implemented in the child classes
        public abstract List<string> PrintStats(); 
        public abstract void RollStats();
        public abstract void GenerateStartingItems();
        public abstract void LevelUp();
        public abstract List<KeyValuePair<string, Action>> PrintSkillList();        
        public abstract void Attack(BaseClass attacker);
       
        
        public List<string> PrintCharacter() //Returns a list of strings that contains the character's info, that are used to print the character's stats in the UI
        {
            return new List<string>
            {
                $" Name: {Name}",
                $" Class: {ClassName}",
                $" Level: {Level}",
                $" XP: {Experience}",
                $" Health: {Health}/{MaxHealth}",
                $" Mana: {Mana}/{MaxMana}"
            };
        }

        public List<string> PrintMonster() //Returns a list of strings that contains the monster's stats, that are used to print the monster's stats in the UI
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



        public List<string> PrintRolledStats() //Only used in character creation. Returns a list of strings that contains the character's rolled stats, that are used to print the character's stats in the UI
        {
             return new List<string> { 
                $"Health: {Health}/{MaxHealth}",
                $"Mana: {Mana}/{MaxMana}",
                $"Strength: (10-20) {BaseStrength}",
                $"Dexterity: (7-15) {BaseDexterity}",
                $"Intelligence: (5-13) {BaseIntelligence}" };
        }

        public List<string> PrintEquipment() //Returns a list of strings that contains the character's equipment, that are used to print the character's stats in the UI
        {
            List<string> items = new List<string>();
            Equipment.ForEach(x => items.Add(x.ToString()));
            return items;

        }

        public List<string> PrintBag() //Returns a list of strings that contains the character's items, that are used to print the character's stats in the UI
        {
            List<string> bags = new List<string>();
            Bag.ForEach(x => bags.Add(x.ToString()));
            return bags;
        }

        public List<KeyValuePair<string, Action>> AddItemsAsOptions() //Returns a list of KeyValuePairs that contains the items name and the action that is used to use the item.
        {
            List<KeyValuePair<string, Action>> bag = new List<KeyValuePair<string, Action>>();
            foreach (var item in Bag)
            {
                bag.Add(new KeyValuePair<string, Action>($"{bag.Count +1}. {item.Name}", item.Use ));
            }
            return bag;
        }

        public void FullBag()
        {
            if (HolderClass.Instance.ChosenClass.Bag.Count >= 10)
            {
                PrintUI.SplitLog($"You have too many items in your bag. You need to drop an item.");
                HolderClass.Instance.Options.Clear();
                foreach (var item in HolderClass.Instance.ChosenClass.Bag)
                {
                    HolderClass.Instance.Options.Add(new KeyValuePair<string, Action>($"{HolderClass.Instance.ChosenClass.Bag.IndexOf(item) + 1}. {item.Name}", item.RemoveItem));
                }
                PrintUI.Print();

            }

        }

        public void LoadSkillsFromLSaveFile(List<string> skills) //This method uses reflection to reload the skills from the save file. The skills are saved as text in the save file. 
        {
            SkillList.Clear();
            string directory = "";
            foreach (var skill in skills)
            {
                Directory.GetDirectories(@"..\..\..\Skills").ToList().ForEach(x =>
                {
                    Directory.GetFiles(x).ToList().ForEach(y =>
                    {
                        if (Path.GetFileNameWithoutExtension(y) == skill)
                        {
                            directory = $"DungeonMaster.Skills.{ Path.GetFileName(x)}.{Path.GetFileNameWithoutExtension(y)}";

                            
                        }
                    });
                });
                if (directory == "") continue;
                Type type = Type.GetType(directory);
                if (type == null) continue;
                var skillInstance = (BaseSkill)Activator.CreateInstance(type);
                if (skillInstance != null)
                {
                    SkillList.Add(new KeyValuePair<string, Action>($"{SkillList.Count + 1}. {skill}", skillInstance.UseSkill));
                }
            }
        }
    }
}
