using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMaster.Equipment
{
    public interface IEquipment
    {
        string Name { get; set; }
        int Strength { get; set; }
        int Dexterity { get; set; }
        int Intelligence { get; set; }
        bool HasEffect { get; set; }    
        void Effect();
    }
}
