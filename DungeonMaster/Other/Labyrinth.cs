using DungeonMaster.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMaster.Other
{
    /// <summary>
    /// Handles the labyrinth generation and movement.
    /// </summary>  
    public static class Labyrinth
    {
        private static int _currentX, _currentY; //The current coordinates
        private static int _counter; //The counter for the number of rooms
        private static bool[,] _haveChecked; //Array to keep track of if a room has been checked

        public static int numberX { get; set; } = 15; //The number of rooms in the x direction
        public static int numberY { get; set; } = 8; //The number of rooms in the y direction

        public static (int x, int y) StartingCoordinates; //The starting coordinates of the labyrinth
        private static List<List<bool>> _map; //2D list to keep track if a room is empty or not
        public static bool[,] CanTeleport = new bool[numberX, numberY]; //2D array to keep track which rooms are accesible
        public static List<List<string>> List { get; set; } // 2D List used to print the labyrinth

        private static List<string> desc = new List<string>();

        public static void CreateLabyrinth()
        {
            HolderClass.Instance.HasStairs = false;
            while (!makeLab() && !HolderClass.Instance.HasStairs) ;
            HolderClass.Instance.FloorLevel++;
            LabToList();
        } //Method to create a new labyrinth

        public static void AddOptions()
        {
            HolderClass.Instance.DirectionalOptions = new List<KeyValuePair<string, Action<string>>>            
            {
                new KeyValuePair<string, Action<string>>("1. \u2190", Direction),
                new KeyValuePair<string, Action<string>>("2. \u2191", Direction),
                new KeyValuePair<string, Action<string>>("3. \u2192", Direction),
                new KeyValuePair<string, Action<string>>("4. \u2193", Direction),
            };

        } //Adds directional options to the UI

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
        } //Converts the labyrinth to a 2D list for printing

        public static void SetRoomToSolved() //Set the current room to solved, so that it doesnt trigger options again
        {
            HolderClass.Instance.Rooms[_currentX][_currentY].IsSolved = true;
        }

        public static void SetCoordinates(int x, int y) //Sets the new coordinates after teleporting
        {
            _currentX = x;
            _currentY = y;
        }

        public static (int x, int y) GetCoordinates() => (_currentX, _currentY); //Returns the current coordinates

        public static bool makeLab() //The method that creates the labyrinth
        {
            _haveChecked = new bool[numberX, numberY]; 
            if (List != null || _map != null) //Clears the lists in preparation for a new labyrinth
            {
                Array.Clear(CanTeleport, 0, CanTeleport.Length);
                List.Clear();
                _map.Clear();
            }
            List = new List<List<string>>();
            _map = new List<List<bool>>();
            HolderClass.Instance.Rooms = new List<List<Room>>();
            _counter = 0; //Counter that keeps tracks of the number of rooms you are able to walk to.
            Random r = new Random();
            for (int i = 0; i < numberX; i++) //Nested loop that randomly adds rooms
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
            while (!StartingPoint()) ; //Executes the startingpoint method until it is set in a position that has a room
            if (Fibo(_currentX, _currentY) > 10) //Counts the numberd of connected rooms
            {
                AddBossAndStairs(); //Add boss room and stair
                return true;
            }

            else
            {
                HolderClass.Instance.HasStairs = false;
                return false;
            }

        }

        private static void AddBossAndStairs() //Adds the boss and stairs room to a room that is connected to the starting point
        {
            Random rnd = new Random();
            int stairsX = 0;
            int stairsY = 0;
            int bossX = 0;
            int bossY = 0;
            do
            {
               stairsX = rnd.Next(HolderClass.Instance.Rooms.Count);
               stairsY = rnd.Next(HolderClass.Instance.Rooms[stairsX].Count);
            } while (!CanTeleport[stairsX, stairsY] || HolderClass.Instance.Rooms[stairsX][stairsY].CurrentEvent is Stairs); //Retries until a room is found that is not the starting point.
            do
            {
                bossX = rnd.Next(HolderClass.Instance.Rooms.Count);
                bossY = rnd.Next(HolderClass.Instance.Rooms[bossX].Count);
            } while (!CanTeleport[bossX, bossY] || HolderClass.Instance.Rooms[bossX][bossY].CurrentEvent is Stairs); //Retries until a room is found that is not the starting point or the stairs room.
            HolderClass.Instance.Rooms[stairsX][stairsY].CurrentEvent = new Stairs(); //Sets the stairs room
            HolderClass.Instance.Rooms[bossX][bossY].CurrentEvent = new Boss(); //Sets the boss room
            HolderClass.Instance.IsBossDead = false;
            HolderClass.Instance.HasEnteredBossRoom = false;
            HolderClass.Instance.HasEnteredStairs = false;
        }

        private static bool StartingPoint() //Sets the starting point of the labyrinth, Returns true if a valid room is found
        {
            Random r = new Random();
            _currentX = r.Next(_map.Count);
            _currentY = r.Next(_map[0].Count);
            if (_map[_currentX][_currentY] == true)
            {
                List[_currentX][_currentY] = "\u25CF";
                HolderClass.Instance.Rooms[_currentX][_currentY].IsFirstRoom = true;
                HolderClass.Instance.Rooms[_currentX][_currentY].CurrentEvent = new Stairs(true);
                SetRoomToSolved(); //Sets the starting room to solved so it doesnt trigger the stairs options
                
                //var description = HolderClass.Instance.Rooms[_currentX][_currentY].CurrentEvent.Description;
                //int index = description.FindIndex(x => x == "");
                //description.RemoveRange(index + 1, description.Count - index - 1);
                //description.Add("You see the opening above you. There is now way up!");
                StartingCoordinates = (_currentX, _currentY);
                return true;
            }
            else
            {
                return false;
            }
        }


        public static void Direction(string direction) //Method that handles the movement of the player. Accepts both 1-4 or arrow keys
        {
            if (direction == "Up")
            {
                if (_currentY > 0)
                {
                    if (_map[_currentX][_currentY - 1] == true)
                    {
                        HolderClass.Instance.HasMoved = true; 
                        _currentY--;
                        List[_currentX][_currentY] = "\u25CF";
                        List[_currentX][_currentY + 1] = HolderClass.Instance.Rooms[_currentX][_currentY + 1].Icon;
                    }
                    else 
                    { 
                        List[_currentX][_currentY - 1] = "\u2588";
                        HolderClass.Instance.HasMoved = false; //
                    }

                } 
                else HolderClass.Instance.HasMoved = false;
            }
            else if (direction == "Down")
            {
                if (_currentY < _map[0].Count - 1)
                {
                    if (_map[_currentX][_currentY + 1] == true)
                    {
                        HolderClass.Instance.HasMoved = true;
                        _currentY++;
                        List[_currentX][_currentY] = "\u25CF";
                        List[_currentX][_currentY - 1] = HolderClass.Instance.Rooms[_currentX][_currentY - 1].Icon;
                    }
                    else
                    { 
                        List[_currentX][_currentY + 1] = "\u2588";
                        HolderClass.Instance.HasMoved = false;
                    }
                }
                else HolderClass.Instance.HasMoved = false;
            }
            else if (direction == "Left")
            {
                if (_currentX > 0)
                {
                    if (_map[_currentX - 1][_currentY] == true)
                    {
                        HolderClass.Instance.HasMoved = true;
                        _currentX--;
                        List[_currentX][_currentY] = "\u25CF";
                        List[_currentX + 1][_currentY] = HolderClass.Instance.Rooms[_currentX + 1][_currentY].Icon;
                    }
                    else
                    {
                        List[_currentX - 1][_currentY] = "\u2588";
                        HolderClass.Instance.HasMoved = false;
                    }
                }
                else HolderClass.Instance.HasMoved = false;
            }
            else if (direction == "Right")
            {
                if (_currentX < _map.Count - 1)
                {
                    if (_map[_currentX + 1][_currentY] == true)
                    {
                        HolderClass.Instance.HasMoved = true;
                        _currentX++;
                        List[_currentX][_currentY] = "\u25CF";
                        List[_currentX - 1][_currentY] = HolderClass.Instance.Rooms[_currentX - 1][_currentY].Icon;
                    }
                    else
                    {
                        List[_currentX + 1][_currentY] = "\u2588";
                        HolderClass.Instance.HasMoved = false;
                    }
                }
                else HolderClass.Instance.HasMoved = false;
            }
            LabToList();
            HolderClass.Instance.SkipNextPrintOut = true;
        }

        private static int Fibo(int x, int y) //Recursive method that counts the number of rooms connected to the starting point
        {
            int RecursiveX = x;
            int RecursiveY = y;
            CanTeleport[x, y] = true;

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
            if (_counter > 24 && !HolderClass.Instance.HasStairs)
            {
                HolderClass.Instance.Rooms[x][y].CurrentEvent = new Stairs();
            }
            return _counter;
        }

        public static List<string> PrintMap() => desc;

    }
}
