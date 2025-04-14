using DungeonMaster.Descriptions;
using DungeonMaster.Equipment;
using DungeonMaster.Items;
using DungeonMaster.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace DungeonMaster.Events
{
    public class Merchant : IEvent
    {
        public Merchant()
        {
            RoomDescription.GenerateRandomRoomDescription();
            RoomDescription.AddEventText($"{EventText()}", true);
            Description = RoomDescription.GetRandomRoomDescription();
            for (int i = 0; i < 4; i++)
            {
                items.Add(new RandomizedItem());
                equipment.Add(GenerateEquipment.RandomEquipment());
            }
        }

        private List<RandomizedItem> items = new List<RandomizedItem>();
        private List<IEquipment> equipment = new List<IEquipment>();
        private IEquipment currentitem;
        private IEquipment listeditem;

        private string EventText()
        {
            return "You see a merchant in the room. He looks at you with a smile. He has a lot of items for sale.";
        }

        public string Type => "Merchant";
        public List<string> Description { get; set; }

        public void BeforeNextRoom()
        {
            Labyrinth.SetRoomToSolved();
            UpdateEventText();
            HolderClass.Instance.SkipNextPrintOut = true;
        }

        public void UpdateEventText()
        {
            int index = Description.IndexOf("");
            Description.RemoveRange(index + 1, Description.Count - index - 1);
            Description.Add($"The merchant is nowhere to be seen"); 
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
                new KeyValuePair<string, Action>($"1. Buy item", ListItem),
                new KeyValuePair<string, Action>($"2. Buy equipment", ListEquipment),
                new KeyValuePair<string, Action>($"3. Maybe another time", BeforeNextRoom)
            };
        }

        private void ListEquipment()
        {
            HolderClass.Instance.Options.Clear();
            for (int i = 0; i < equipment.Count; i++)
            {
                int index = i;
                HolderClass.Instance.Options.Add(new KeyValuePair<string, Action>($"{i + 1}. {equipment[i].Name}", () => InfoEquipment(equipment[index])));
            }
        }

        private void InfoEquipment(IEquipment looteditem)
        {
            currentitem = HolderClass.Instance.ChosenClass.Equipment.FirstOrDefault(x => x.GetType() == looteditem.GetType());
            listeditem = looteditem;
            PrintUI.SplitLog($"You asked to look at the {looteditem.GetType().Name}");
            PrintUI.SplitLog($"It has {looteditem.Strength} strength, {looteditem.Dexterity} dexterity and {looteditem.Intelligence} intelligence");
            PrintUI.SplitLog($"You would {(looteditem.Strength > currentitem.Strength ? "gain" : "lose")} {(looteditem.Strength - currentitem.Strength > 0 ? looteditem.Strength - currentitem.Strength : currentitem.Strength - looteditem.Strength)} strength");
            PrintUI.SplitLog($"You would {(looteditem.Dexterity > currentitem.Dexterity ? "gain" : "lose")} {(looteditem.Dexterity - currentitem.Dexterity > 0 ? looteditem.Dexterity - currentitem.Dexterity : currentitem.Dexterity - looteditem.Dexterity)} dexterity");
            PrintUI.SplitLog($"You would {(looteditem.Intelligence > currentitem.Intelligence ? "gain" : "lose")} {(looteditem.Intelligence - currentitem.Intelligence > 0 ? looteditem.Intelligence - currentitem.Intelligence : currentitem.Intelligence - looteditem.Intelligence)} intelligence");
            HolderClass.Instance.Options.Clear();
            HolderClass.Instance.Options.Add(new KeyValuePair<string, Action>($"{HolderClass.Instance.Options.Count + 1}. Yes", BuyEquipment));
            HolderClass.Instance.Options.Add(new KeyValuePair<string, Action>($"{HolderClass.Instance.Options.Count + 1}. No", Run));
            HolderClass.Instance.OptionsFooter = "Do you want to buy this item?";
            PrintUI.Print();
        }

        private void BuyEquipment()
        {
            HolderClass.Instance.ChosenClass.Equipment[HolderClass.Instance.ChosenClass.Equipment.IndexOf(currentitem)] = listeditem;
            HolderClass.Instance.SkipNextTryChoice = true;
            PrintUI.Print();
            BeforeNextRoom();
        }

        private void ListItem()
        {
            HolderClass.Instance.Options.Clear();
            for (int i = 0; i < items.Count; i++)
            {
                int index = i;
                HolderClass.Instance.Options.Add(new KeyValuePair<string, Action>($"{i + 1}. {items[i].Name}", () => BuyItem(items[index])));
            }

            HolderClass.Instance.Options.Add(new KeyValuePair<string, Action>($"{items.Count + 1}. Go back", Run));
        }

        private void BuyItem(RandomizedItem randomizedItem)
        {
            if (HolderClass.Instance.ChosenClass.Bag.Count >= 10)
            {
                PrintUI.SplitLog("You can't carry more items. You need to drop something first.");
                PrintUI.Print();
                return;
            }
            var addedItem = randomizedItem;
            HolderClass.Instance.ChosenClass.Bag.Add(randomizedItem);
            HolderClass.Instance.ChosenClass.FullBag();
            items.Remove(randomizedItem);
            BeforeNextRoom();
        }

        public void SetUIState()
        {
            HolderClass.Instance.SkipNextPrintOut = false;
        }
    }
}
