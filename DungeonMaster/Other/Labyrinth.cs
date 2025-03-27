using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMaster.Other
{
    public static class Labyrinth
    {
        private static int _currentX, _currentY;
        private static int _counter;
        private static bool[,] _haveChecked;
        //public int y;
        public static int numberX { get; set; } = 15;
        public static int numberY { get; set; } = 8;

        private static List<List<bool>> _map;
        public static List<List<string>> List { get; set; }

        private static List<string> desc = new List<string>();

        public static void CreateLabyrinth()
        {
            while (!makeLab()) ;
            LabToList();
        }

        public static void AddOptions()
        {
            HolderClass.Instance.DirectionalOptions = new List<KeyValuePair<string, Action<string>>>            
            {
                new KeyValuePair<string, Action<string>>("1. \u2190", Direction),
                new KeyValuePair<string, Action<string>>("2. \u2191", Direction),
                new KeyValuePair<string, Action<string>>("3. \u2192", Direction),
                new KeyValuePair<string, Action<string>>("4. \u2193", Direction),
            };
            
        }

        public static void LabToList()
        {
            desc.Clear();
            for (int j = 0; j < List[j].Count; j++)
            {
                desc.Add("");
                for (int i = 0; i < List.Count; i++)
                {
                    desc[j] += List[i][j];
                }
            }
        }

        public static void SetRoomToSolved()
        {
            HolderClass.Instance.Rooms[_currentX][_currentY].IsSolved = true;
        }

        public static void SetCoordinates(int x, int y)
        {
            _currentX = x;
            _currentY = y;
        }

        public static (int x, int y) GetCoordinates() => (_currentX, _currentY);

        public static bool makeLab()
        {
            _haveChecked = new bool[numberX, numberY];
            if (List != null || _map != null)
            {
                List.Clear();
                _map.Clear();
            }
            List = new List<List<string>>();
            _map = new List<List<bool>>();
            HolderClass.Instance.Rooms = new List<List<Room>>();
            _counter = 0;
            Random r = new Random();
            for (int i = 0; i < numberX; i++)
            {
                List.Add(new List<string>());
                _map.Add(new List<bool>());
                HolderClass.Instance.Rooms.Add(new List<Room>());
                for (int j = 0; j < numberY; j++)
                {
                    if (r.Next(100) < 50)
                    {
                        _map[i].Add(true);
                        List[i].Add(" ");
                        HolderClass.Instance.Rooms[i].Add(new Room());
                    }
                    else
                    {
                        _map[i].Add(false);
                        List[i].Add(" ");
                        HolderClass.Instance.Rooms[i].Add(null);
                    }
                }
            }
            while (!StartingPoint()) ;
            if (Fibo(_currentX, _currentY) > 25)
            {
                return true;
            }

            else
            {
                return false;
            }

        }

        private static bool StartingPoint()
        {
            Random r = new Random();
            _currentX = r.Next(_map.Count);
            _currentY = r.Next(_map[0].Count);
            if (_map[_currentX][_currentY] == true)
            {
                List[_currentX][_currentY] = "\u25CF";
                HolderClass.Instance.Rooms[_currentX][_currentY].IsFirstRoom = true;
                var description = HolderClass.Instance.Rooms[_currentX][_currentY].CurrentEvent.Description;
                
                int index = description.FindIndex(x => x == "");
                description.RemoveRange(index + 1, description.Count - index - 1);
                description.Add("You see the opening above you. There is now way up!");
                //description[description.Count - 1] = "You see the opening above you. There is now way up!";

                return true;
            }
            else
            {
                return false;
            }
        }


        public static void Direction(string direction)
        {
            if (direction == "Up")
            {
                if (_currentY > 0)
                {
                    if (_map[_currentX][_currentY - 1] == true)
                    {
                        _currentY--;
                        List[_currentX][_currentY] = "\u25CF";
                        List[_currentX][_currentY + 1] = HolderClass.Instance.Rooms[_currentX][_currentY + 1].Icon;
                    }
                    //else List[_currentX][_currentY - 1] = "\u2500";
                    else List[_currentX][_currentY - 1] = "\u2588";

                }
            }
            else if (direction == "Down")
            {
                if (_currentY < _map[0].Count - 1)
                {
                    if (_map[_currentX][_currentY + 1] == true)
                    {
                        _currentY++;
                        List[_currentX][_currentY] = "\u25CF";
                        List[_currentX][_currentY - 1] = HolderClass.Instance.Rooms[_currentX][_currentY - 1].Icon;
                    }
                    //else List[_currentX][_currentY + 1] = "\u2500";
                    else List[_currentX][_currentY + 1] = "\u2588";
                }
            }
            else if (direction == "Left")
            {
                if (_currentX > 0)
                {
                    if (_map[_currentX - 1][_currentY] == true)
                    {
                        _currentX--;
                        List[_currentX][_currentY] = "\u25CF";
                        List[_currentX + 1][_currentY] = HolderClass.Instance.Rooms[_currentX + 1][_currentY].Icon;
                    }
                    //else List[_currentX - 1][_currentY] = "\u2502";
                    else List[_currentX - 1][_currentY] = "\u2588";
                }
            }
            else if (direction == "Right")
            {
                if (_currentX < _map.Count - 1)
                {
                    if (_map[_currentX + 1][_currentY] == true)
                    {
                        _currentX++;
                        List[_currentX][_currentY] = "\u25CF";
                        List[_currentX - 1][_currentY] = HolderClass.Instance.Rooms[_currentX - 1][_currentY].Icon;
                    }
                    //else List[_currentX + 1][_currentY] = "\u2502";
                    else List[_currentX + 1][_currentY] = "\u2588";
                }
            }
            LabToList();
            HolderClass.Instance.SkipNextPrintOut = true;
        }

        private static int Fibo(int x, int y)
        {
            int RecursiveX = x;
            int RecursiveY = y;
            if (RecursiveX > 0 && _map[RecursiveX - 1][RecursiveY] == true && _haveChecked[RecursiveX - 1, RecursiveY] == false)
            {
                _counter++;
                _haveChecked[RecursiveX - 1, RecursiveY] = true;
                _counter = Fibo(RecursiveX - 1, RecursiveY);
            }
            if (RecursiveY > 0 && _map[RecursiveX][RecursiveY - 1] == true && _haveChecked[RecursiveX, RecursiveY - 1] == false)
            {
                _counter++;
                _haveChecked[RecursiveX, RecursiveY - 1] = true;
                _counter = Fibo(RecursiveX, RecursiveY - 1);
            }
            if (RecursiveX < _map[0].Count - 1 && _map[RecursiveX + 1][RecursiveY] == true && _haveChecked[RecursiveX + 1, RecursiveY] == false)
            {
                _counter++;
                _haveChecked[RecursiveX + 1, RecursiveY] = true;
                _counter = Fibo(RecursiveX + 1, RecursiveY);
            }
            if (RecursiveY < _map[0].Count - 1 && _map[RecursiveX][RecursiveY + 1] == true && _haveChecked[RecursiveX, RecursiveY + 1] == false)
            {
                _counter++;
                _haveChecked[RecursiveX, RecursiveY + 1] = true;
                _counter = Fibo(RecursiveX, RecursiveY + 1);
            }

            return _counter;
        }

        public static List<string> PrintMap() => desc;

    }
}
