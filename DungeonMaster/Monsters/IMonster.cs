using DungeonMaster.Equipment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMaster.Monsters
{
    public interface IMonster
    {
        string Name { get; set; }
        string MonsterName { get; set; }
        int MaxHealth { get; set; }
        int Health { get; set; }
        int MaxMana { get; set; }
        int Mana { get; set; }
        int Strength { get; set; }
        int Dexterity { get; set; }
        int Intelligence { get; set; }
        int Crit { get; set; }
        int Luck { get; set; }
        int Level { get; set; }
        int Armor { get; set; }
        int Resistance { get; set; }
        double PhysicalDamageResist { get; set; }
        double MagicalDamageResist { get; set; }
        void CalculateMaxHealth();
        void CalculateMaxMana();
        void CalculateCrit();
        void CalculateLuck();
        void CalculatePhysicalDamageResist();
        void CalculateMagicalDamageResist();
        void RollStats();
        string Skill1();
        string Skill2();
        string Skill3();
        string Skill4();
    }
}
