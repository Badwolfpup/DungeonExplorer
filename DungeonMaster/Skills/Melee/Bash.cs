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
        public override int RemainingRounds { get; set; } = 0; // The number of rounds the skill is active for

        public override int ManaCost { get; set; } = 40; // The mana cost of the skill

        private BaseClass Player => HolderClass.Instance.ChosenClass; //Simplify access to the player class
        private BaseClass Monster => HolderClass.Instance.Monster; //Simplify access to the monster class

        public override void EndSkill() => HolderClass.Instance.ChosenClass.DamageDoneModifier = 1; // Reset the damage done modifier to 1 when the skill ends

        public override bool MonsterUseSkill() // This method is called when the monster uses the skill
        {
            // If the skill is still active, the monster will attack the player
            if (RemainingRounds > 0)
            {
                var monster = Monster as Monster;
                monster.Attack(Player);
                RemainingRounds--;
                return false;
            }

            // If the skill is not active, the monster will use the skill
            PrintUI.SplitLog("The enemy bashes the pommel of it's weapon in your head. Somethings feels off. You feel your strength draining");
            RemainingRounds = 3;
            Player.DamageDoneModifier = 0.75;
            int damage = 20 + (int)(HolderClass.Instance.Monster.Strength * Player.DamageTakenModifier * Player.AltarDamageTakenModifier * Monster.DamageDoneModifier * Player.DamageResist);
            Player.Health -= damage;
            PrintUI.SplitLog($"The {Monster.Name} deal {damage} damage");
            return true;
        }

        public override void UseSkill() // This method is called when the player uses the skill
        {
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
                Monster.DamageDoneModifier = 0.75;
                int damage = 20 + (int)(HolderClass.Instance.ChosenClass.Strength * Player.DamageDoneModifier * Player.AltarDamageDoneModifier * Monster.DamageTakenModifier * Monster.DamageResist);
                Monster.Health -= damage;
                PrintUI.SplitLog($"You deal {damage} damage");
                if (Monster.Health > 0) PrintUI.SplitLog($"The {Monster.Name} has {Monster.Health} hp left");
                else
                {
                    PrintUI.SplitLog($"The {Monster.Name} has been defeated!");
                }
                HolderClass.Instance.IsPlayerTurn = !HolderClass.Instance.IsPlayerTurn;
            }
        }
    }
}
