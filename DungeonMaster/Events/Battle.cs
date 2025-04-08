using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml.Linq;
using DungeonMaster.Classes;
using DungeonMaster.Descriptions;
using DungeonMaster.Equipment;
using DungeonMaster.Items;
using DungeonMaster.Other;
using DungeonMaster.Skills;
using DungeonMaster.Skills.Melee;

namespace DungeonMaster.Events
{
    public class Battle : IEvent
    {
        #region PropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion

        public BaseClass ChosenClass => HolderClass.Instance.ChosenClass;
        public BaseClass Monster => HolderClass.Instance.Monster;
        public string Type { get; } = "Battle";
        public List<string> Description { get; set; }
        private bool playersturn = true;
        public string MonsterText { get; set; }
        public Monster monster { get; set; }

        public Battle()
        {
            RoomDescription.GenerateRandomRoomDescription();
            MonsterText = RandomizeMonster();
            RoomDescription.AddEventText($" You run into a {MonsterText}", true);
            Description = RoomDescription.GetRandomRoomDescription();
        }

        public Battle(bool boss)
        {

        }



        public void Run()
        {
            HolderClass.Instance.Monster = monster;
            SetUIState();
            SetDefaultOptions();

            StartEvent();
        }

        private void StartEvent()
        {

            while (Monster.Health > 0 && ChosenClass.Health > 0)
            {
                if (HolderClass.Instance.IsPlayerTurn)
                {
                    if (HolderClass.Instance.PlayerUsedAction)
                    {
                        ChosenClass.Effects.ForEach(x =>
                        {
                            BaseSkill skill = (BaseSkill)x.Target;
                            if (skill == null) return;
                            if (skill.RemainingRounds > 0) skill.RemainingRounds--;
                            else skill.EndSkill();
                        });
                        ChosenClass.Effects.RemoveAll(x => (BaseSkill)x.Target is BaseSkill skill && skill.RemainingRounds == 0);
                        SetDefaultOptions();
                        PrintUI.Print();
                    }
                }
                else
                {
                    Random rnd = new Random();
                    if (Monster.Health <= 0) continue;
                    Monster monster = (Monster)Monster;
                    Action<BaseClass> monsteraction = rnd.Next(1, 101) switch
                    {
                        < 70 => monster.Attack,
                        < 101 => monster.UseAbility,
                        _ => monster.Attack
                    };

                    monsteraction?.Invoke(ChosenClass);
                    HolderClass.Instance.IsPlayerTurn = !HolderClass.Instance.IsPlayerTurn;
                    PrintUI.SplitLog("");
                }
                HolderClass.Instance.Turn++;
            }

            if (Monster.Health <= 0)
            {
                WonBattle();
            }
            else
            {
                PrintUI.SplitLog("You have died.");
                
            }
        }

        public void SetUIState()
        {
            HolderClass.Instance.ShowLog = true;
            HolderClass.Instance.ShowMonster = true;
            HolderClass.Instance.Turn = 1;
            PrintUI.CombatLog.Clear();
        }

        private void WonBattle()
        {
            PrintUI.SplitLog($"You have defeated the {Monster.Name}");
            if (Monster.Level < ChosenClass.Level - 3)
            {
                PrintUI.SplitLog($"The monster level was too low for you to gain any experience points");
            } else
            {
                int newxp = Monster.Level <= ChosenClass.Level ? 100 - ((ChosenClass.Level - Monster.Level) * 2) : 10 + ((Monster.Level - ChosenClass.Level) * 3);
                ChosenClass.Experience += newxp;
                PrintUI.SplitLog($"You have gained {newxp} experience points. ");

                if (ChosenClass.Experience >= 100)
                {                  
                    ChosenClass.LevelUp(); 

                }
                PrintUI.SplitLog($"You need {100 - ChosenClass.Experience} xp to level up");
 
                Loot();
            }
        }

        private void Loot()
        {
            Labyrinth.SetRoomToSolved();
            //var coordinates = Labyrinth.GetCoordinates();
            //HolderClass.Instance.Rooms[coordinates.x][coordinates.y].IsSolved = true;
            HolderClass.Instance.IsPlayerTurn = true;
            Random rnd = new Random();
            int loot = rnd.Next(1, 101);
            if (loot < 1)
            {
                RandomizedItem item = new RandomizedItem();
                ChosenClass.Bag.Add(item);
                PrintUI.SplitLog($"You have found {(Regex.IsMatch(item.Name[0].ToString(), @"^[aeiouAEIOU]") ? "an" : "a")} {item.Name}");
                SkipPrintOut();
            }
            else if (loot < 170)
            {
                IEquipment looteditem = GenerateEquipment.RandomEquipment();
                //switch (rnd.Next(1, 4))
                //{
                //    case 1:
                //        looteditem = GenerateEquipment.Chest(ChosenClass.ClassName, ChosenClass.Level);
                //        break;
                //    case 2:
                //        looteditem = GenerateEquipment.Head(ChosenClass.ClassName, ChosenClass.Level);
                //        break;
                //    case 3:
                //        looteditem = GenerateEquipment.Weapon(ChosenClass.ClassName, ChosenClass.Level);
                //        break;
                //    default:
                //        looteditem = new Weapon("Wooden sword");
                //        break;
                //}
                PrintUI.SplitLog($"You have found a new {looteditem.GetType().Name}");
                PrintUI.SplitLog($"It has {looteditem.Strength} strength, {looteditem.Dexterity} dexterity and {looteditem.Intelligence} intelligence");
                var currentitem = ChosenClass.Equipment.FirstOrDefault(x => x.GetType() == looteditem.GetType());
                PrintUI.SplitLog($"You would {(looteditem.Strength > currentitem.Strength ? "gain" : "lose")} {(looteditem.Strength - currentitem.Strength > 0 ? looteditem.Strength - currentitem.Strength : currentitem.Strength - looteditem.Strength)} strength");
                PrintUI.SplitLog($"You would {(looteditem.Dexterity > currentitem.Dexterity ? "gain" : "lose")} {(looteditem.Dexterity - currentitem.Dexterity > 0 ? looteditem.Dexterity - currentitem.Dexterity : currentitem.Dexterity - looteditem.Dexterity)} dexterity");
                PrintUI.SplitLog($"You would {(looteditem.Intelligence > currentitem.Intelligence ? "gain" : "lose")} {(looteditem.Intelligence - currentitem.Intelligence > 0 ? looteditem.Intelligence - currentitem.Intelligence : currentitem.Intelligence - looteditem.Intelligence)} intelligence");
                HolderClass.Instance.Options.Clear();
                HolderClass.Instance.Options.Add(new KeyValuePair<string, Action>($"{HolderClass.Instance.Options.Count + 1}. Yes", () => EquipNewItem(looteditem, currentitem)));
                HolderClass.Instance.Options.Add(new KeyValuePair<string, Action>($"{HolderClass.Instance.Options.Count + 1}. No", () => SkipPrintOut()));
                PrintUI.Print();
                if (!HolderClass.Instance.IsBossFight) UpdateEventText();
            }
            BeforeNextRoom();

        }

        public void UpdateEventText()
        {
            int index = Description.IndexOf("");
            Description.RemoveRange(index + 1, Description.Count - index - 1);
            Description.Add($"You see the corpse of the {Monster.Name} lying on the floor");
        }

        public void BeforeNextRoom()
        {
            PrintUI.SplitLog("");
            PrintUI.SplitLog("Press any key to continue...");
            PrintUI.SplitLog("");
            PrintUI.Print();
            HolderClass.Instance.ChosenClass.ResetAltarBuffsDebuffs();
            string input = Console.ReadKey(true).KeyChar.ToString();
        }

        private void SkipPrintOut()
        {
            HolderClass.Instance.SkipNextPrintOut = true;

        }

        private void EquipNewItem(IEquipment newitem, IEquipment olditem)
        {
            ChosenClass.Equipment[ChosenClass.Equipment.IndexOf(olditem)] = newitem;
            HolderClass.Instance.SkipNextPrintOut = true;
        }

        public void SetDefaultOptions()
        { 
            ChosenClass.IsUsingAbility = false;
            HolderClass.Instance.Options = new List<KeyValuePair<string, Action>>()
            {
                new KeyValuePair<string, Action>("1. Attack", Attack),
                new KeyValuePair<string, Action>("2. Use ability", UseAbility),
                new KeyValuePair<string, Action>("3. Use item", UseItem),
                new KeyValuePair<string, Action>("4. Run away", RunAway)
            };
        }

        private string RandomizeMonster()
        {
            Random r = new Random();
            string name = MonsterNames.RandomMonsterName();
            string monstername = MonsterNames.RandomMonsterFullName(name);
            monster = new Monster(name, "", HolderClass.Instance.FloorLevel);
            HolderClass.Instance.HasMonster = true;
            return monstername;
        }



        private void RunAway()
        {
            Random random = new Random();
            int escapechange = 30 + ChosenClass.Dexterity - (Monster.Level - ChosenClass.Level > 0 ? Monster.Level - ChosenClass.Level * 10 : 0);
            if (random.Next(100) < escapechange)
            {
                PrintUI.SplitLog("You have successfully escaped");
                PrintUI.SplitLog("You run away from the monster");
            }
            else
            {
                PrintUI.SplitLog("You have failed to escape");
                PrintUI.SplitLog("You stumble and fall to the ground unable to act for a moment");
                
            }
        }

        private void UseItem()
        {
            //ChosenClass.IsUsingAbility = false;
            HolderClass.Instance.Options.Clear();
            ChosenClass.PrintUseItems().ForEach(x => HolderClass.Instance.Options.Add(x));
            HolderClass.Instance.Options.Add(new KeyValuePair<string, Action>($"{HolderClass.Instance.Options.Count + 1}. Go back to previous menu", () => SetDefaultOptions()));
        }

        private void UseAbility()
        {
            HolderClass.Instance.Options.Clear();
            ChosenClass.SkillList.ForEach(x =>
            {
                BaseSkill temp = x.Value.Target as BaseSkill;
                //Laddas bara när man väljer use abilirty...
                x = new KeyValuePair<string, Action>($"{ChosenClass.SkillList.Count}. {x.Value.Target.GetType().Name} " +
                    $"{(temp.RemainingRounds > 0 ? $"({temp.RemainingRounds} rounds left.)" : "")}",  x.Value);
                HolderClass.Instance.Options.Add(x);

            });
            HolderClass.Instance.Options.Add(new KeyValuePair<string, Action>($"{HolderClass.Instance.Options.Count + 1}. Go back to previous menu", () => SetDefaultOptions()));
        }

        private void Attack()
        {
            ChosenClass.IsUsingAbility = false;
            ChosenClass.Attack(Monster);
        }



    }
}
