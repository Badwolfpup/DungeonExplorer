using DungeonMaster.Descriptions;
using DungeonMaster.Equipment;
using DungeonMaster.Items;
using DungeonMaster.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DungeonMaster.Events
{
    public class Prisoner : IEvent
    {
        Random rnd = new Random();
        private int typeofevent;
        private string monstername;
        Battle battle;

        public Prisoner()
        {
            typeofevent = rnd.Next(2);
            battle = new Battle();
            RoomDescription.GenerateRandomRoomDescription();
            RoomDescription.AddEventText($"{EventText()}", true);
            Description = RoomDescription.GetRandomRoomDescription();
            
        }

        private string EventText()
        {
            return $"You see a {battle.monster.Name} in a cell. He looks at you with a pleading look. He is chained to the wall. He looks like he has been here for a long time.";
        }

        public string Type => "Prisoner";
        public List<string> Description { get; set; }

        public void BeforeNextRoom()
        {
            Labyrinth.SetRoomToSolved();
            HolderClass.Instance.SkipNextPrintOut = true;
        }

        public void Run()
        {
            SetUIState();
            SetDefaultOptions();
            PrintUI.Print();
        }

        public void SetDefaultOptions()
        {
            HolderClass.Instance.Options = new List<KeyValuePair<string, Action>>()
            {
                new KeyValuePair<string, Action>($"1. You feel merciful and release him.", typeofevent == 0 ? Loot : Fight),
                new KeyValuePair<string, Action>($"2. Let him rot. I aint helping filth", BeforeNextRoom),
            };
        }

        private void Fight()
        {
            PrintUI.SplitLog(battle.MonsterText);
            PrintUI.SplitLog("Press any key to continue...");
            HolderClass.Instance.SkipNextTryChoice = true;
            PrintUI.Print();
            HolderClass.Instance.SkipNextTryChoice = false;
            string input = Console.ReadKey(true).KeyChar.ToString();
            battle.Run();
            HolderClass.Instance.SkipNextTryChoice = true;
        }

        private void Loot()
        {
            int loot = rnd.Next(2);
            if (loot == 0) NewLoot(GenerateEquipment.RandomEquipment());
            else if (loot == 1) NewItem();
            BeforeNextRoom();
        }

        private void NewItem()
        {
            RandomizedItem item = new RandomizedItem();
            HolderClass.Instance.ChosenClass.Bag.Add(item);
            PrintUI.SplitLog($"The {HolderClass.Instance.Monster.Name} thanks you profusely and offer you an item as a reward.");
            PrintUI.SplitLog($"You have recieved {(Regex.IsMatch(item.Name[0].ToString(), @"^[aeiouAEIOU]") ? "an" : "a")} {item.Name}");
            HolderClass.Instance.ChosenClass.FullBag();
            HolderClass.Instance.SkipNextTryChoice = true;
            PrintUI.Print();
            //BeforeNextRoom();
        }

        private void NewLoot(IEquipment looteditem)
        {
            var currentitem = HolderClass.Instance.ChosenClass.Equipment.FirstOrDefault(x => x.GetType() == looteditem.GetType());
            if (currentitem != null)
            {
                PrintUI.SplitLog($"The {HolderClass.Instance.Monster.Name} thanks you profusely and offer you an piece of equipment as a reward.");
                PrintUI.SplitLog($"You have received a new {looteditem.GetType().Name}");
                PrintUI.SplitLog($"It has {looteditem.Strength} strength, {looteditem.Dexterity} dexterity and {looteditem.Intelligence} intelligence");
                PrintUI.SplitLog($"You would {(looteditem.Strength > currentitem.Strength ? "gain" : "lose")} {(looteditem.Strength - currentitem.Strength > 0 ? looteditem.Strength - currentitem.Strength : currentitem.Strength - looteditem.Strength)} strength");
                PrintUI.SplitLog($"You would {(looteditem.Dexterity > currentitem.Dexterity ? "gain" : "lose")} {(looteditem.Dexterity - currentitem.Dexterity > 0 ? looteditem.Dexterity - currentitem.Dexterity : currentitem.Dexterity - looteditem.Dexterity)} dexterity");
                PrintUI.SplitLog($"You would {(looteditem.Intelligence > currentitem.Intelligence ? "gain" : "lose")} {(looteditem.Intelligence - currentitem.Intelligence > 0 ? looteditem.Intelligence - currentitem.Intelligence : currentitem.Intelligence - looteditem.Intelligence)} intelligence");
                HolderClass.Instance.Options.Clear();
                HolderClass.Instance.Options.Add(new KeyValuePair<string, Action>($"{HolderClass.Instance.Options.Count + 1}. Yes", () => EquipNewItem(looteditem, currentitem)));
                HolderClass.Instance.Options.Add(new KeyValuePair<string, Action>($"{HolderClass.Instance.Options.Count + 1}. No", () => HolderClass.Instance.SkipNextPrintOut = true));
            } else
            {
                HolderClass.Instance.ChosenClass.Equipment.Add(looteditem);
                PrintUI.SplitLog($"The {HolderClass.Instance.Monster.Name} thanks you profusely and offer you an piece of equipment as a reward.");
                PrintUI.SplitLog($"You have received a new {looteditem.GetType().Name}");
                PrintUI.SplitLog($"It has {looteditem.Strength} strength, {looteditem.Dexterity} dexterity and {looteditem.Intelligence} intelligence");
                HolderClass.Instance.SkipNextTryChoice = true;
            }
            PrintUI.Print();
        }

        private void EquipNewItem(IEquipment newitem, IEquipment olditem)
        {
            HolderClass.Instance.ChosenClass.Equipment[HolderClass.Instance.ChosenClass.Equipment.IndexOf(olditem)] = newitem;
            HolderClass.Instance.SkipNextTryChoice = true;
            PrintUI.Print();
            //BeforeNextRoom();
        }

        public void SetUIState()
        {
            HolderClass.Instance.SkipNextPrintOut = false;
        }
    }
}
