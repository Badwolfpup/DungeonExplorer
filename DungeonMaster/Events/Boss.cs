using DungeonMaster.Classes;
using DungeonMaster.Descriptions;
using DungeonMaster.Other;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMaster.Events
{
    public class Boss : IEvent
    {
        Battle battle;
        public Boss()
        {
            battle = new Battle(true); //Creates a new battle with the boss constructor
            Description = battle.Description; //Gets the description from the Battle object
        }


        public string Type => "Boss"; // Type of event
        public List<string> Description { get; set; }

        public void BeforeNextRoom() // The method that is called before moving on to the next room
        {
            if (HolderClass.Instance.IsBossDead)
            {
                Labyrinth.SetRoomToSolved();
                HolderClass.Instance.SkipNextPrintOut = true;
                HolderClass.Instance.IsBossFight = false;
                HolderClass.Instance.IsBossDead = true;
                if (HolderClass.Instance.ChosenClass.Health > 0) UpdateEventText();
            }
        }

        public void UpdateEventText() //This method updates the event portion of the room description 
        {
            int index = Description.IndexOf("");
            Description.RemoveRange(index + 1, Description.Count - index - 1);
            Description.Add($"You see the corpse of the {HolderClass.Instance.Monster.Name} lying on the floor");
        }

        public void Run() //Executes the event
        {
            HolderClass.Instance.HasEnteredBossRoom = true;
            battle.Run();
            BeforeNextRoom();
        }

        public void SetDefaultOptions()
        {
        }

        public void SetUIState()
        {
        }
    }
}
