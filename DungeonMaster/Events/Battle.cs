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

        public BaseClass ChosenClass => HolderClass.Instance.ChosenClass; //Simplifed access to the chosen class
        public BaseClass Monster => HolderClass.Instance.Monster; //Simplified access to the monster
        public string Type { get; } = "Battle"; //Event type
        public List<string> Description { get; set; } //Description of the event
        private bool hasrunaway; //Used to check if the player has run away from the battle
        private (int x, int y) coords; //Coordinates of the room
        public string MonsterText { get; set; } //Text to display the monster name
        public Monster monster { get; set; } //Monster object hat holds the monster create

        //Constructor for regular battle
        public Battle()
        {
            RoomDescription.GenerateRandomRoomDescription();
            MonsterText = RandomizeMonster();
            RoomDescription.AddEventText($" You run into a {MonsterText}", true);
            Description = RoomDescription.GetRandomRoomDescription();
            
        }

        //Constructor for boss battle
        public Battle(bool boss)
        {
            RoomDescription.GenerateRandomRoomDescription();
            RoomDescription.AddEventText($" A powerful aura emanates from {RandomizeBoss()}. He looks at you with a menacing look. He looks like he alot tougher than the other monsters.", true);
            Description = RoomDescription.GetRandomRoomDescription();
        }



        public void Run()
        {
            coords = Labyrinth.GetCoordinates(); //Get the coordinates of the room
            HolderClass.Instance.IsBossFight = HolderClass.Instance.Rooms[coords.x][coords.y].CurrentEvent is Boss; //Checks if the current event is a boss
            HolderClass.Instance.Monster = monster; //Set the monster to the current monster
            SetUIState();
            SetDefaultOptions();

            StartEvent();
        }

        private void StartEvent()
        {

            while ((Monster.Health > 0 && ChosenClass.Health > 0) && !hasrunaway) //Contonues the battle until either has died or the player successfully runs away
            {
                if (HolderClass.Instance.IsPlayerTurn)
                {
                    if (HolderClass.Instance.PlayerUsedAction) //Iff the player didnt use an action, e.g. opened a menu it will loop back to the player
                    {
                        HolderClass.Instance.SkipNextTryChoice = false;
                        //Loops through all active effects and reduces/ends them
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
                    Monster monster = (Monster)Monster; //Cast the monster to an instance of Monster, instead of BaseClass

                    //Randomize the monster's action and saves it in a Action delegate
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
                if (HolderClass.Instance.IsBossFight) HolderClass.Instance.IsBossDead = true;
                WonBattle();
            }
            else if (!hasrunaway)
            {
                PrintUI.SplitLog("You have died.");
                
            }
            if (HolderClass.Instance.IsBossFight) hasrunaway = false;
        }

        public void SetUIState()
        {
            HolderClass.Instance.ShowLog = true;
            HolderClass.Instance.ShowMonster = true;
            HolderClass.Instance.Turn = 1;
            PrintUI.CombatLog.Clear();
        }

        //Handles xp and loot after the battle
        private void WonBattle()
        {
            PrintUI.SplitLog($"You have defeated the {Monster.Name}");
            if (Monster.Level < ChosenClass.Level - 3) //If the monster is too low level, the player will not gain any xp
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

        
        private void Loot() //Randomizes the loot after the battle, either item or equipment
        {
            Labyrinth.SetRoomToSolved();
            HolderClass.Instance.IsPlayerTurn = true;
            Random rnd = new Random();
            int loot = rnd.Next(1, 101);
            if (loot < 50)
            {
                RandomizedItem item = new RandomizedItem();
                PrintUI.SplitLog($"You have found {(Regex.IsMatch(item.Name[0].ToString(), @"^[aeiouAEIOU]") ? "an" : "a")} {item.Name}");

                ChosenClass.Bag.Add(item);
                ChosenClass.FullBag();
                SkipPrintOut();
            }
            else if (loot < 50)
            {
                IEquipment looteditem = GenerateEquipment.RandomEquipment();
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
            if (!hasrunaway) Description.Add($"You see the corpse of the {Monster.Name} lying on the floor");
            else Description.Add($"The {Monster.Name} is nowhere to be seen");
        } //Updates the text of the monster after it's been defeated or the player has run away

        public void BeforeNextRoom() //Adds a pause before moving on to the next room
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

        private void EquipNewItem(IEquipment newitem, IEquipment olditem) //Equips the new item. 
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
        } //Randomizes the monster name and creates a new monster object

        private string RandomizeBoss()
        {
            Random r = new Random();
            string name = MonsterNames.RandomBossName();
            monster = new Monster(name, "", HolderClass.Instance.FloorLevel);
            monster.BossStats();
            HolderClass.Instance.HasMonster = true;
            return name;
        } //Randomizes the boss name and creates a new monster object



        private void RunAway() //Handles the run away action
        {
            Random random = new Random();
            int escapechange = 30 + ChosenClass.Dexterity - (Monster.Level - ChosenClass.Level > 0 ? Monster.Level - ChosenClass.Level * 10 : 0); //Calculates the chance to escape
            HolderClass.Instance.SkipNextTryChoice = true;
            if (random.Next(100) < escapechange)
            {
                var coords = Labyrinth.GetCoordinates();    
                if (!HolderClass.Instance.IsBossFight) 
                {
                    PrintUI.SplitLog("You have successfully escaped");
                    PrintUI.SplitLog("You run away from the monster");
                    hasrunaway = true;
                    UpdateEventText(); 
                    BeforeNextRoom();
                } else
                {
                    PrintUI.SplitLog("You have successfully escaped");
                    PrintUI.SplitLog("You have a feeling the boss will be waiting for you though");
                    hasrunaway = true;
                    BeforeNextRoom();
                }
            }
            else
            {
                PrintUI.SplitLog("You have failed to escape");
                PrintUI.SplitLog("You stumble and fall to the ground unable to act for a moment");
                HolderClass.Instance.IsPlayerTurn = !HolderClass.Instance.IsPlayerTurn;
            }
        }

        private void UseItem()
        {
            //ChosenClass.IsUsingAbility = false;
            HolderClass.Instance.Options.Clear();
            ChosenClass.AddItemsAsOptions().ForEach(x => HolderClass.Instance.Options.Add(x));
            HolderClass.Instance.Options.Add(new KeyValuePair<string, Action>($"{HolderClass.Instance.Options.Count + 1}. Go back to previous menu", () => SetDefaultOptions()));
        } //Adds the items as options to the options menu

        private void UseAbility() //Add the skills as options to the options menu
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

        private void Attack() //Calls the attack method of the chosen class
        {
            ChosenClass.IsUsingAbility = false;
            ChosenClass.Attack(Monster);
        }



    }
}
