using DungeonMaster.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMaster.Skills
{
    /// <summary>
    /// Base class for all skills. Contains the properties and methods that are used by all skills.
    /// </summary>
    public abstract class BaseSkill
    {
        public abstract int RemainingRounds { get; set; }
        public abstract int ManaCost { get; set; }
        public abstract void UseSkill();
        public abstract void EndSkill();
        public abstract bool MonsterUseSkill();

    }
}
