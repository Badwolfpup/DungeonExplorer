using DungeonMaster.Descriptions;
using DungeonMaster.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMaster.Events
{
    public class Altar : IEvent
    {
        public string Type => "Altar";
        public List<string> Description { get; set; }

        public Altar()
        {
            RoomDescription.GenerateRandomRoomDescription();
            RoomDescription.AddEventText($"{EventText()}", true);
            Description = RoomDescription.GetRandomRoomDescription();
        }

        private object EventText()
        {
            return $"You see an old abandoned altar. You wonder what kind of deity was worshipped here";
        } //Contains the text for the event


        public void BeforeNextRoom() //This is called when the player has finished the event and is ready to move on
        {
            Labyrinth.SetRoomToSolved();
            HolderClass.Instance.SkipNextPrintOut = true;
        }

        public void Run()
        {
            SetUIState();
            SetDefaultOptions();
            PrintUI.Print();
        } //Main method that runs the event

        public void SetDefaultOptions()
        {
            HolderClass.Instance.Options = new List<KeyValuePair<string, Action>>()
            {
                new KeyValuePair<string, Action>($"1. You pray at the altar. Hopefully the deity is benign.", Pray),
                new KeyValuePair<string, Action>($"2. I don't want to to take any chances", BeforeNextRoom),
            };
        } //Sets the default options for the event. This is called when the event is first created

        private void Pray() //Eventspecific method. Determines if player is buffed or debuffed and what type of buff/debuff
        {
            Random rnd = new Random();
            if (rnd.Next(3) > 0)
            {
                int x = rnd.Next(3);
                if (x == 0)
                {
                    HolderClass.Instance.ChosenClass.AltarDamageDoneModifier = 1.25;
                    PrintUI.SplitLog("You feel stronger. You will do more damage");
                }
                else if (x == 1)
                {
                    PrintUI.SplitLog("You feel stronger. Your strength, dexterity and intelligence have gone up.");
                    HolderClass.Instance.ChosenClass.AltarStrModifier = 1.25;
                    HolderClass.Instance.ChosenClass.AltarDexModifier = 1.25;
                    HolderClass.Instance.ChosenClass.AltarIntModifier = 1.25;
                }
                else
                {
                    PrintUI.SplitLog("You feel stronger. Your will be more likely to crit.");
                    HolderClass.Instance.ChosenClass.AltarCritModifier = 1.25;
                }
            } else
            {
                int x = rnd.Next(3);
                if (x == 0)
                {
                    HolderClass.Instance.ChosenClass.AltarDamageDoneModifier = 0.75;
                    PrintUI.SplitLog("You feel weaker. You will do less damage");
                }
                else if (x == 1)
                {
                    PrintUI.SplitLog("You feel weaker. Your strength, dexterity and intelligence have gone down.");
                    HolderClass.Instance.ChosenClass.AltarStrModifier = 0.75;
                    HolderClass.Instance.ChosenClass.AltarDexModifier = 0.75;
                    HolderClass.Instance.ChosenClass.AltarIntModifier = 0.75;
                }
                else
                {
                    PrintUI.SplitLog("You feel weaker. Your will be less likely to crit.");
                    HolderClass.Instance.ChosenClass.AltarCritModifier = 0.75;
                }
            }
            BeforeNextRoom();
        }

        public void SetUIState() //This is called when the event is first created. This is where chnges to the UI state is made.
        {
            HolderClass.Instance.SkipNextPrintOut = false;
        }
    }
}
