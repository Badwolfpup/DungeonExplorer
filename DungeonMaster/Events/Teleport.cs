using DungeonMaster.Descriptions;
using DungeonMaster.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMaster.Events
{
    public class Teleport : IEvent
    {
        public Teleport()
        {
            RoomDescription.GenerateRandomRoomDescription();
            RoomDescription.AddEventText($"You stepped on a tile a hear a click. Suddenly the room start to swirl and you find yourlself somewhere else. You feel weaker. Press any key to continue", true);
            Description = RoomDescription.GetRandomRoomDescription();
        }

        public string Type => "Teleport";
        public List<string> Description { get; set; }

        public void Run()
        {
            //HolderClass.Instance.ShowOptions = false;
            //HolderClass.Instance.SkipNextTryChoice = true;
            SetUIState();
            PrintUI.Print();
            string input = Console.ReadKey(true).KeyChar.ToString();
            SetUIState();
            //HolderClass.Instance.ShowOptions = true;
            //HolderClass.Instance.SkipNextTryChoice = false;

            HolderClass.Instance.ChosenClass.Health -= (int)(HolderClass.Instance.ChosenClass.Health * 0.2);
            while (!RandomLocation()) ;
        }

        private bool RandomLocation()
        {
            Random rnd = new Random();
            int x = rnd.Next(HolderClass.Instance.Rooms.Count);
            int y = rnd.Next(HolderClass.Instance.Rooms[x].Count);
            var coord = Labyrinth.GetCoordinates();
            if (HolderClass.Instance.Rooms[x][y] == null) return false;
            if (coord.x != x && coord.y != y)
            {
                Labyrinth.List[x][y] = "\u25CF";
                HolderClass.Instance.Rooms[coord.x][coord.y].IsSolved = true;

                Labyrinth.List[coord.x][coord.y] = HolderClass.Instance.Rooms[coord.x][coord.y].Icon;
                Labyrinth.SetCoordinates(x, y);
                Labyrinth.LabToList();
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SetDefaultOptions()
        {

        }

        public void SetUIState()
        {
            HolderClass.Instance.ShowOptions = !HolderClass.Instance.ShowOptions;
            HolderClass.Instance.SkipNextTryChoice = !HolderClass.Instance.SkipNextTryChoice;
        }

        public void BeforeNextRoom()
        {
            throw new NotImplementedException();
        }
    }
}
