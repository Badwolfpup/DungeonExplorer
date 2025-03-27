using DungeonMaster.Descriptions;
using DungeonMaster.Equipment;
using DungeonMaster.Items;
using DungeonMaster.Other;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace DungeonMaster.Events
{
    public class Treasure : IEvent
    {
        Random rnd = new Random();
        private int typeofevent;
        private int randomTreasure;
        public string Type => "Treasure";
        public List<string> Description { get; set; }

        public Treasure()
        {
            typeofevent = rnd.Next(2);
            randomTreasure = rnd.Next(2);
            RoomDescription.GenerateRandomRoomDescription();
            if (typeofevent == 0) RoomDescription.AddEventText($"Next to one of the walls you see a derelict chest. It has some kind of lock mechanism. It looks like" +
                $"{(randomTreasure == 0 ? "dexterity" : "intelligence")} will be useful when trying to open it", true);
            else RoomDescription.AddEventText($"In the dark corner of the room you spot something that looks like a treasure. Maybe you will finally be rich!", true);
            Description = RoomDescription.GetRandomRoomDescription();
        }


        public void Run()
        {
            SetUIState();
            SetDefaultOptions();
            PrintUI.Print();
        }

        public void SetDefaultOptions()
        {
            if (typeofevent == 0)
            {
                HolderClass.Instance.Options = new List<KeyValuePair<string, Action>>()
                {
                    new KeyValuePair<string, Action>($"1. Pick the lock ({(randomTreasure == 0 ? "dex" : "int")}) ", PickLock),
                    new KeyValuePair<string, Action>($"2. Smash the chest (str)", SmashChest),
                    new KeyValuePair<string, Action>($"3. I've seen how this ends. No, thank you!", BeforeNextRoom),
                };
            }
            else
            {
                HolderClass.Instance.Options = new List<KeyValuePair<string, Action>>()
                {
                    new KeyValuePair<string, Action>($"1. Go get the treasure ", GetTreasure),
                    new KeyValuePair<string, Action>($"2. No, god knows what's lurking in the shadows", BeforeNextRoom),
                };
            }
        }

        private void GetTreasure()
        {   
            if (rnd.Next(100) < 0)
            {
                int loot = rnd.Next(2);
                if (loot == 0) NewLoot(GenerateEquipment.RandomEquipment());
                else if (loot == 1) NewItem();
                BeforeNextRoom();
            }
            else
            {
                Battle battle = new Battle();
                PrintUI.SplitLog(battle.MonsterText);
                PrintUI.SplitLog("Press any key to continue...");
                HolderClass.Instance.SkipNextTryChoice = true;
                PrintUI.Print();
                HolderClass.Instance.SkipNextTryChoice = false;
                string input = Console.ReadKey(true).KeyChar.ToString();
                battle.Run();
                HolderClass.Instance.SkipNextTryChoice = true;
            }
            
        }

        public void BeforeNextRoom()
        {
            HolderClass.Instance.SkipNextPrintOut = true;
            Labyrinth.SetRoomToSolved();
        }

        private void SmashChest()
        {
            if (rnd.Next(100) < 10 + (int)(HolderClass.Instance.ChosenClass.Strength * 0.1)) GetLoot(false);
            else TrapTriggered();
        }

        private void PickLock()
        {
            if (rnd.Next(100) < 25 + (int)((randomTreasure == 0 ? HolderClass.Instance.ChosenClass.Dexterity : HolderClass.Instance.ChosenClass.Intelligence) * 0.1)) GetLoot(true);
            else TrapTriggered();
        }

        private void TrapTriggered()
        {
            int damage = 50 + HolderClass.Instance.ChosenClass.MaxHealth / 10;
            HolderClass.Instance.ChosenClass.Health -= damage;
            PrintUI.SplitLog($"You triggered a trap! You take {damage} damage");
            HolderClass.Instance.SkipNextTryChoice = true;
            PrintUI.Print();
            BeforeNextRoom();
        }

        private void GetLoot(bool method)
        {
            string text = ""; 
            int gold = rnd.Next(30, 101);
            int loot = rnd.Next(3);

            text = $"{(method ? "You managed to open the chest." : "You smashed the chest open.")} Inside you find {gold} gold.";
            if (loot == 0) NewLoot(GenerateEquipment.RandomEquipment());
            else if (loot == 1) NewItem();
        }

        private void NewItem()
        {
            RandomizedItem item = new RandomizedItem();
            HolderClass.Instance.ChosenClass.Bag.Add(item);
            PrintUI.SplitLog($"You have found {(Regex.IsMatch(item.Name[0].ToString(), @"^[aeiouAEIOU]") ? "an" : "a")} {item.Name}");
            HolderClass.Instance.SkipNextTryChoice = true;
            PrintUI.Print();
            BeforeNextRoom();
        }

        private void NewLoot(IEquipment looteditem)
        {
            var currentitem = HolderClass.Instance.ChosenClass.Equipment.FirstOrDefault(x => x.GetType() == looteditem.GetType());
            PrintUI.SplitLog($"You have found a new {looteditem.GetType().Name}");
            PrintUI.SplitLog($"It has {looteditem.Strength} strength, {looteditem.Dexterity} dexterity and {looteditem.Intelligence} intelligence");
            PrintUI.SplitLog($"You would {(looteditem.Strength > currentitem.Strength ? "gain" : "lose")} {(looteditem.Strength - currentitem.Strength > 0 ? looteditem.Strength - currentitem.Strength : currentitem.Strength - looteditem.Strength)} strength");
            PrintUI.SplitLog($"You would {(looteditem.Dexterity > currentitem.Dexterity ? "gain" : "lose")} {(looteditem.Dexterity - currentitem.Dexterity > 0 ? looteditem.Dexterity - currentitem.Dexterity : currentitem.Dexterity - looteditem.Dexterity)} dexterity");
            PrintUI.SplitLog($"You would {(looteditem.Intelligence > currentitem.Intelligence ? "gain" : "lose")} {(looteditem.Intelligence - currentitem.Intelligence > 0 ? looteditem.Intelligence - currentitem.Intelligence : currentitem.Intelligence - looteditem.Intelligence)} intelligence");
            HolderClass.Instance.Options.Clear();
            HolderClass.Instance.Options.Add(new KeyValuePair<string, Action>($"{HolderClass.Instance.Options.Count + 1}. Yes", () => EquipNewItem(looteditem, currentitem)));
            HolderClass.Instance.Options.Add(new KeyValuePair<string, Action>($"{HolderClass.Instance.Options.Count + 1}. No", () => SkipPrintOut()));
            PrintUI.Print();
        }

        private void EquipNewItem(IEquipment newitem, IEquipment olditem)
        {
            HolderClass.Instance.ChosenClass.Equipment[HolderClass.Instance.ChosenClass.Equipment.IndexOf(olditem)] = newitem;
            HolderClass.Instance.SkipNextTryChoice = true;
            PrintUI.Print();
            BeforeNextRoom();
        }

        private void SkipPrintOut()
        {
            HolderClass.Instance.SkipNextPrintOut = true;

        }

        public void SetUIState()
        {
            HolderClass.Instance.ShowLog = true;
            HolderClass.Instance.SkipNextPrintOut = false;
        }
    }
}
