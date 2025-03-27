using DungeonMaster.Classes;
using DungeonMaster.Descriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMaster.Skills.Melee
{
    public class Bash : BaseSkill
    {
        public override int RemainingRounds { get; set; } = 0;

        public override int ManaCost { get; set; } = 40;

        private BaseClass Player => HolderClass.Instance.ChosenClass;
        private BaseClass Monster => HolderClass.Instance.Monster;

        public override void EndSkill() => HolderClass.Instance.ChosenClass.DamageDoneModifier = 1;

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
            PrintUI.SplitLog("The enemy bashes the pommel of it's weapon in your head. Somethings feels off. You feel your strength draning");
            //Monster.Effects.Add(MonsterUseSkill);
            RemainingRounds = 3;
            Player.DamageDoneModifier = 0.75;
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
            Player.Effects.Add(UseSkill);
            Player.IsUsingAbility = true;
            HolderClass.Instance.PlayerUsedAction = true;

            PrintUI.SplitLog("You bash the enemy with the pommel of your weapon. It seems to have dazed it. Maybe it won't be as effective murdering you for awhile.");
            int damage = 20 + HolderClass.Instance.ChosenClass.Strength;
            Monster.DamageDoneModifier = 0.75;
            Monster.Health -= damage;
            PrintUI.SplitLog($"You deal {damage} damage");
            if (Monster.Health >0) PrintUI.SplitLog($"The {Monster.Name} has {Monster.Health} hp left");
            else
            {
                PrintUI.SplitLog($"The {Monster.Name} has been defeated!");
            }
            HolderClass.Instance.IsPlayerTurn = !HolderClass.Instance.IsPlayerTurn;
        }
    }
}
