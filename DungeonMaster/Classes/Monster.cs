using DungeonMaster.Descriptions;
using DungeonMaster.Equipment;
using DungeonMaster.Skills;
using Newtonsoft.Json.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Linq;

namespace DungeonMaster.Classes
{
    public class Monster : BaseClass
    {
        public override List<BaseSkill> Skills { get; set; }
        private int floorlevel;
        public Monster(string name, string className, int floorlevel) : base(name, className)
        {
            this.floorlevel = floorlevel;
            GenerateStartingEquipment();
            RollStats();
            LoadSkills();
            AddNewSkill();
        }

        public override double DamageResist => (Strength * 0.7 + Dexterity * 0.3 + 100) / 1000;

        public override void GenerateStartingItems()
        {
            throw new NotImplementedException();
        }

        public override void LevelUp()
        {
            throw new NotImplementedException();
        }

        public override List<KeyValuePair<string, Action>> PrintSkillList()
        {
            throw new NotImplementedException();
        }

        public override void RollStats()
        {
            Random rnd = new Random();
            Level = floorlevel + rnd.Next(1,3);
            BaseStrength = rnd.Next(10, 21) * Level;
            BaseDexterity = rnd.Next(7, 16) * Level;
            BaseIntelligence = rnd.Next(5, 14) * Level;
            //Health = MaxHealth;
            Health = 100;
            Mana = MaxMana;
            
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

        private void LoadSkills()
        {
            Random rnd = new Random();
            Skills = new List<BaseSkill>();
            var folders = Directory.GetDirectories(Path.GetFullPath(@"..\..\..\Skills"));
            string path = Path.GetFullPath(folders[rnd.Next(folders.Length)]);
            path = Path.GetFullPath(@"..\..\..\Skills\Melee");
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

        public void UseAbility(BaseClass player)
        {
            if (Skills.Count > 1)
            {
                Random rnd = new Random();
                Skills[rnd.Next(Skills.Count)].MonsterUseSkill();
            } else { Skills[0].MonsterUseSkill(); }
        }

        public override void Attack(BaseClass player)
        {
            Random rnd = new Random();
            double x = rnd.Next(30);
            double variance = 1.0 + (x / 100);

            int damage = (int)Math.Round((5 + Strength * StrModifier) / 2 * DamageDoneModifier * variance * (1 - player.DamageResist) * player.DamageTakenModifier);
            player.Health -= (int)damage;
            var Monster = HolderClass.Instance.Monster;
            PrintUI.SplitLog($"The monster swings it's weapon {(Monster.Health > 0 ? $"It deals {damage} damage. You have {player.Health} hp left" : $"It deals {damage} damage. You havw died")}");

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
                if (newskill != null) SkillList.Add(new KeyValuePair<string, Action>($"{SkillList.Count + 1}. {newskill.Method.DeclaringType.Name} ", newskill));
            }
        }

        public override List<string> PrintStats()
        {
            throw new NotImplementedException();
        }
    }
}