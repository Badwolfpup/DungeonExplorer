using DungeonMaster.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMaster.Other
{
    public class SaveLoad
    {
        public string CLassType { get; set; }
        public string Name { get; set; }
        public List<string> Skills { get; set; } = new List<string>();
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Intelligence { get; set; }
        public int DungeonFloor { get; set; }
        public int Experience { get; set; }
        public int Health { get; set; }
        public int Mana { get; set; }
        public int Level { get; set; }   

        public void Save()
        {
            CLassType = HolderClass.Instance.ChosenClass.GetType().Name;
            Name = HolderClass.Instance.ChosenClass.Name;
            Strength = HolderClass.Instance.ChosenClass.Strength;
            Dexterity = HolderClass.Instance.ChosenClass.Dexterity;
            Intelligence = HolderClass.Instance.ChosenClass.Intelligence;
            DungeonFloor = HolderClass.Instance.FloorLevel;
            Experience = HolderClass.Instance.ChosenClass.Experience;
            Health = HolderClass.Instance.ChosenClass.Health;
            Mana = HolderClass.Instance.ChosenClass.Mana;
            Level = HolderClass.Instance.ChosenClass.Level;
            string _filepath = AppDomain.CurrentDomain.BaseDirectory + "Savefile.json";
            AddSkills();

            try
            {
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All,
                    PreserveReferencesHandling = PreserveReferencesHandling.All,
                    Formatting = Formatting.Indented

                };
                string json = JsonConvert.SerializeObject(this, settings);
                File.WriteAllText(_filepath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in Save method: {ex.Message}");
                throw;
            }
        }

        private void AddSkills()
        {
            HolderClass.Instance.ChosenClass.SkillList.ForEach(x =>
            {
                Skills.Add(x.Value.Method.DeclaringType.Name);
            });
        }

        public static bool Load(out BaseClass b)
        {
            string _filepath = AppDomain.CurrentDomain.BaseDirectory + "Savefile.json";
            if (!File.Exists(_filepath)) { b = new Warrior("", ""); return false; }

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                PreserveReferencesHandling = PreserveReferencesHandling.All
            };

            string file = File.ReadAllText(_filepath);
            if (!string.IsNullOrWhiteSpace(file))
            {
                var save = JsonConvert.DeserializeObject<SaveLoad>(file, settings);
                if (save.CLassType == "Warrior")
                {
                    Type type = Type.GetType($"DungeonMaster.Classes.{save.CLassType}");
                    if (type != null)
                    {
                        //var klass = new Warrior(save.Name, save.CLassType);
                        BaseClass klass = Activator.CreateInstance(type, save.Name, save.CLassType) as BaseClass;
                        klass.BaseStrength = save.Strength;
                        klass.BaseDexterity = save.Dexterity;
                        klass.BaseIntelligence = save.Intelligence;
                        klass.Experience = save.Experience;
                        klass.Level = save.Level;
                        klass.Health = save.Health;
                        klass.Mana = save.Mana;
                        HolderClass.Instance.FloorLevel = save.DungeonFloor;
                        klass.LoadSkillsFromLSaveFile(save.Skills);
                        b = klass;
                        return true;
                    }
                }
            }
            b = new Warrior("", "");
            return false;

        }

    }
}
