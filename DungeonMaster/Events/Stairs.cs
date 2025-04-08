using DungeonMaster.Descriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMaster.Events
{
    public class Stairs : IEvent
    {
        Random rnd = new Random();
        int typeofevent;
        string type;
        public Stairs()
        {
            type = "Stairs";
            HolderClass.Instance.HasStairs = true;
            typeofevent = rnd.Next(3);
            RoomDescription.GenerateRandomRoomDescription();
            RoomDescription.AddEventText($"{EventText()}", true);
            Description = RoomDescription.GetRandomRoomDescription();
        }

        public Stairs(bool first)
        {
            type = "StartingPoint";
            RoomDescription.GenerateRandomRoomDescription();
            RoomDescription.AddEventText($"You see the remains of the rope you used slide down. Above you, you see the opening - unreachable. You have no choice but to cotinue on", true);
            Description = RoomDescription.GetRandomRoomDescription();
        }

        private string EventText()
        {
            
            switch (typeofevent)
            {
                case 0: return "You see a staircase leading down. It's dark and you can't see the bottom. Do you dare to go down?";
                case 1: return "You see a opening in the floor. A slab of stone has been moved. There is a derelict rope ladder going down.";
                case 2: return "You hear a sharp crack and you start to fall down. The floor must have been weakened. Maybe it's a trap";
                default: return "You see a staircase leading down. It's dark and you can't see the bottom. Do you dare to go down?";
            }
        }

        public string Type => type;
        public List<string> Description { get; set; }

        public void BeforeNextRoom()
        {
            HolderClass.Instance.SkipNextPrintOut = true;
            HolderClass.Instance.IsNewFloor = true;
            HolderClass.Instance.Save();
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
                new KeyValuePair<string, Action>($"1. Go down to the next floor", BeforeNextRoom),
                new KeyValuePair<string, Action>($"2. Continue to explore this floor", () => { HolderClass.Instance.SkipNextPrintOut = true; }),
            };
        }

        public void SetUIState()
        {
            HolderClass.Instance.SkipNextPrintOut = false;
        }
    }
}
