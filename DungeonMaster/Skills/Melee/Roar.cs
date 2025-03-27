using DungeonMaster.Classes;
using DungeonMaster.Descriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMaster.Skills.Melee
{
    public class Roar: BaseSkill
    {
        public override int RemainingRounds { get; set; } = 0;
        public override int ManaCost { get; set; } = 30;

        private BaseClass Player => HolderClass.Instance.ChosenClass;
        private BaseClass Monster => HolderClass.Instance.Monster;

        public override void EndSkill() => HolderClass.Instance.ChosenClass.StrModifier = 1;

        public override bool MonsterUseSkill()
        {
            if (RemainingRounds > 0)
            {
                var monster = Monster as Monster;
                monster.Attack(HolderClass.Instance.ChosenClass);
                RemainingRounds--;
                return false;
            }
            RemainingRounds = 3;
            PrintUI.SplitLog($"The enemy roars. It looks more intimidating now.");
            //Monster.Effects.Add(MonsterUseSkill);
            RemainingRounds = 3;
            Monster.StrModifier = 1.5;
            return true;    
        }

        public override void UseSkill()
        {
            if (HolderClass.Instance.ChosenClass.Mana < ManaCost)
            {
                PrintUI.SplitLog("You don't have enough mana to use this skill");
                HolderClass.Instance.PlayerUsedAction = false;
                return;
            }
            if (RemainingRounds > 0)
            {
                PrintUI.SplitLog($"You have already used this skill. You can use it again in {RemainingRounds} turns.");
                HolderClass.Instance.PlayerUsedAction = false;
                return;
            }
            Player.IsUsingAbility = true;
            HolderClass.Instance.PlayerUsedAction = true;
            Player.Effects.Add(UseSkill);
            RemainingRounds = 3;
            Player.StrModifier = 1.5;
            PrintUI.SplitLog("You roar at the enemy");
            PrintUI.SplitLog("You feel yourself growing stronger");
            HolderClass.Instance.IsPlayerTurn = !HolderClass.Instance.IsPlayerTurn;
        }
    }
}
