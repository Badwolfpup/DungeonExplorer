using DungeonMaster.Classes;
using DungeonMaster.Descriptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMaster.Skills.Melee
{
    public class Slam : BaseSkill
    {
        public override int RemainingRounds { get; set; } = 0;

        public override int ManaCost { get; set; } = 25;

        private BaseClass Player => HolderClass.Instance.ChosenClass;
        private BaseClass Monster => HolderClass.Instance.Monster;

        public override void EndSkill()
        {
            
        }

        public override bool MonsterUseSkill()
        {
            if (RemainingRounds > 0) { 
                var monster = Monster as Monster; 
                monster.Attack(HolderClass.Instance.ChosenClass);
                RemainingRounds--;
                return false;
            }
            RemainingRounds = 3;
            PrintUI.SplitLog($"{Monster.Name} slam you with the pommel of its weapon");
            int damage = (int)((20 + Monster.Strength) * Player.DamageTakenModifier * Player.AltarDamageTakenModifier * Monster.DamageDoneModifier * Player.DamageResist);
            Player.Health -= damage;
            PrintUI.SplitLog($"The {Monster.Name} deal {damage} damage to you");
            PrintUI.SplitLog($"You have {(Player.Health > 0 ? $"{Player.Health}" : "0")} hp left");
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
            HolderClass.Instance.ChosenClass.IsUsingAbility = true;
            HolderClass.Instance.PlayerUsedAction = true;
            PrintUI.SplitLog("You slam the enemy with pommel of your weapon");
            int damage = 20 + (int)(HolderClass.Instance.ChosenClass.Strength * Player.DamageDoneModifier * Player.AltarDamageDoneModifier * Monster.DamageTakenModifier * Monster.DamageResist);
            HolderClass.Instance.Monster.Health -= damage;
            PrintUI.SplitLog($"You deal {damage} damage");
            if (HolderClass.Instance.Monster.Health > 0) PrintUI.SplitLog($"The {HolderClass.Instance.Monster.Name} has {HolderClass.Instance.Monster.Health} hp left");
            else
            {
                PrintUI.SplitLog($"The {HolderClass.Instance.Monster.Name} has been defeated!");
            }
            HolderClass.Instance.IsPlayerTurn = !HolderClass.Instance.IsPlayerTurn;
        }
    }
}
