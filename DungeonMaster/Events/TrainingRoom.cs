using DungeonMaster.Descriptions;
using DungeonMaster.Other;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMaster.Events
{
    public class TrainingRoom : IEvent
    {
        public string Type => "Training";

        private Random rnd = new Random();
        private int chosenstat;
        private int session = 3;

        public TrainingRoom()
        {
            chosenstat = rnd.Next(3);
            RoomDescription.GenerateRandomRoomDescription();           
            RoomDescription.AddEventText($"{TrainingText()}", true);
            Description = RoomDescription.GetRandomRoomDescription();
        }



        public List<string> Description { get; set; }
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
                new KeyValuePair<string, Action>($"1. Yes ({session} sessions left)", ImproveStat),
                new KeyValuePair<string, Action>("2. No", BeforeNextRoom),

            };
            if (session > 0)
            {
                HolderClass.Instance.OptionsFooter = $"Improve {GetStat()} by 10 for the cost of {(GetStat() == "Strength" ? "20" : "10")}% of Max HP: ";
            }
            else
            {
                HolderClass.Instance.SkipNextPrintOut = true;
                BeforeNextRoom();
            }

        }

        private string GetStat()
        {
            switch (chosenstat)
            {
                case 0: return "Strength";
                case 1: return "Dexterity";
                case 2: return "Intelligence";
                default: return "Nothing";
            }
        }

        public void SetUIState()
        {
            HolderClass.Instance.SkipNextPrintOut = false;
        }

        public void BeforeNextRoom()
        {
            HolderClass.Instance.SkipNextPrintOut = true;
            Labyrinth.SetRoomToSolved();
        }

        private void ImproveStat()
        {
            session--;
            switch (chosenstat)
            {
                case 0: HolderClass.Instance.ChosenClass.BaseStrength += 10; 
                        HolderClass.Instance.ChosenClass.Health -= HolderClass.Instance.ChosenClass.Health - (int)(HolderClass.Instance.ChosenClass.MaxHealth * 0.1) > 0 ? (int)(HolderClass.Instance.ChosenClass.MaxHealth * 0.2) : 1;
                    break;
                case 1: HolderClass.Instance.ChosenClass.BaseDexterity += 10;
                        HolderClass.Instance.ChosenClass.Health -= HolderClass.Instance.ChosenClass.Health - (int)(HolderClass.Instance.ChosenClass.MaxHealth * 0.1) > 0 ? (int)(HolderClass.Instance.ChosenClass.MaxHealth * 0.1) : 1;
                        break;
                case 2: HolderClass.Instance.ChosenClass.BaseIntelligence += 10;
                        HolderClass.Instance.ChosenClass.Health -= HolderClass.Instance.ChosenClass.Health - (int)(HolderClass.Instance.ChosenClass.MaxHealth * 0.1) > 0 ? (int)(HolderClass.Instance.ChosenClass.MaxHealth * 0.1) : 1;
                        break;
            }
            SetDefaultOptions();
        }

        private string TrainingText()
        {
            Random random = new Random();
            switch (chosenstat)
            {
                case 0: return "On the floor you see some old dumbbells. You can improve your strength with those.";
                case 1: return "Lying in the corner there is a balance disc. You could use that to improve your dexterity.";
                case 2: return "In the middle of the room there is a bookshelf. You could read some books to improve your intelligence.";
                default: return "You see nothing of interest in the room.";
            }
        }
    }
}
