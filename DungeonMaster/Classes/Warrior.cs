using DungeonMaster.Descriptions;
using DungeonMaster.Equipment;
using DungeonMaster.Skills;
using DungeonMaster.Skills.Melee;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DungeonMaster.Classes
{
    public class Warrior : BaseClass
    {

        public override List<BaseSkill> Skills { get; set; }
        public override double DamageResist => (Strength * 0.7 + Dexterity * 0.3 + 100) / 1000;

        
        public Warrior(string name, string classname) : base(name, classname)
        {
            GenerateStartingEquipment();
            GenerateStartingItems();
            LoadSkills();
            AddNewSkill();

        }


        private void LoadSkills()
        {
            Skills = new List<BaseSkill>();
            string path = Path.GetFullPath(@"..\..\..\Skills\Melee");
            if (Directory.Exists(path))
            {
                Directory.GetFiles(path).ToList().ForEach(x =>
                {
                    string name = Path.GetFileNameWithoutExtension(x);
                    Type type = Type.GetType("DungeonMaster.Skills.Melee." + name);
                    var skill = (BaseSkill)Activator.CreateInstance(type);
                    if (skill != null)
                    {
                        Skills.Add(skill);
                    }

                });
            }
        }



        private Action GetSkill()
        {
            Random rnd = new Random();
            int skill = rnd.Next(0, Skills.Count);
            return Skills[skill].UseSkill;
        }

        private void AddNewSkill()
        {
            if (SkillList.Count < Skills.Count)
            {
                bool foundskill = false;
                Action newskill = null;
                do
                {
                    if (SkillList.Count == 4) break;
                    newskill = GetSkill();
                    if (!SkillList.Any(x => x.Key.Contains(newskill.Method.Name)))
                    {
                        foundskill = true;
                    }
                } while (!foundskill);
                if (newskill != null) SkillList.Add(new KeyValuePair<string, Action>($"{SkillList.Count + 1}. {newskill.Method.DeclaringType.Name} ",  newskill));
            }
        }

        public override List<string> PrintStats()
        {
            int str = $"Strength: {Strength}".Length;
            int dex = $"Dexterity: {Dexterity}".Length;
            int inte = $"Intelligence: {Intelligence}".Length;
            return new List<string> {
                $"Strength: {Strength}",
                $"Dexterity: {Dexterity}",
                $"Intelligence: {Intelligence}",
                $"Crit: {Crit}%",
                $"Luck: {Luck}",
                $"Damage resist: {(int)(DamageResist * 100)}%"
            };
        }

        public override void RollStats()
        {
            Random rnd = new Random();
            
            BaseStrength = rnd.Next(10, 21);
            BaseDexterity = rnd.Next(7, 16);
            BaseIntelligence = rnd.Next(5, 14);
            Health = MaxHealth;
            Mana = MaxMana;
            Level = 1;
            Experience = 0;
            Gold = 0;
            SkillList = PrintSkillList();
        }

        public override void LevelUp()
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
            int skillup = rnd.Next(SkillList.Count);
            if (Level % 5 == 0) AddNewSkill();
            if (skilluppchance > 19) _skillevels[skillup]++;
            PrintUI.SplitLog($"You have leveled up! You are now level {Level}! You gained {strgain} strength, {dexgain} dexterity and {intgain} intelligence " +
                $"{(skilluppchance < 49 ? "You didn't gain any skillevels this time" : $"You leveled up your {SkillList[skillup].Key} skill to level {_skillevels[skillup]}")}");
        }

        public override List<KeyValuePair<string, Action>> PrintSkillList() => SkillList;

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






        public override void Attack(BaseClass monster)
        {
            Random rnd = new Random();
            double x = rnd.Next(30);
            double variance = 1.0 + (x / 100);
            
            int damage = (int)Math.Round((5 + Strength * StrModifier) / 2 * DamageDoneModifier * variance * (1 - monster.DamageResist) * monster.DamageTakenModifier);
            monster.Health -= (int)damage;
            int lifesteal = (int)Math.Round(damage * 0.1);
            
            var Monster = HolderClass.Instance.Monster;
            PrintUI.SplitLog($"You swing your {Equipment.FirstOrDefault(x => x is Weapon).Name} {(Monster.Health > 0 ? $"You deal {damage} damage. The {Monster.Name} has {Monster.Health} hp left" : $"You deal {damage} damage. The {Monster.Name} has died")}");
            PrintUI.SplitLog($"You heal for {(MaxHealth - Health >= lifesteal ? lifesteal : MaxHealth-Health)} hit points");
            Health = Health + lifesteal <= MaxHealth ? Health + lifesteal : MaxHealth;
            HolderClass.Instance.IsPlayerTurn = !HolderClass.Instance.IsPlayerTurn;
        }


    }
}
