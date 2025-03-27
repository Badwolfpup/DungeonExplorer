using DungeonMaster.Classes;
using DungeonMaster.Descriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMaster.Skills.Melee
{
    public class Focus : BaseSkill
    {
        public override int RemainingRounds { get; set; } = 0;
        public override int ManaCost { get; set; } = 30;

        private BaseClass Player => HolderClass.Instance.ChosenClass;
        private BaseClass Monster => HolderClass.Instance.Monster;

        public override void EndSkill() => Player.DamageTakenModifier = 1;

        public override bool MonsterUseSkill()
        {
            if (RemainingRounds > 0)
            {
                var monster = Monster as Monster;
                monster.Attack(Player);
                RemainingRounds--;
                return false;
            }
            RemainingRounds = 3;
            PrintUI.SplitLog($"The enemy seems to be focusing intently on something. You get the feeling it'll be harder to kill it.");
            //Monster.Effects.Add(MonsterUseSkill);
            RemainingRounds = 3;
            Monster.DamageTakenModifier = 0.75;
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
            RemainingRounds = 3;
            Player.IsUsingAbility = true;
            HolderClass.Instance.PlayerUsedAction = true;
            Player.Effects.Add(UseSkill);
            Player.DamageTakenModifier = 0.75;
            PrintUI.SplitLog("You take a deep breath and start channeling your inner chi");
            HolderClass.Instance.IsPlayerTurn = !HolderClass.Instance.IsPlayerTurn;
        }
        
    }
}
