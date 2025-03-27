using DungeonMaster.Classes;
using DungeonMaster.Descriptions;
using DungeonMaster.Events;
using DungeonMaster.Other;
using System;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace DungeonMaster;

class Program
{

    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        StartGame start = new StartGame();
        if (HolderClass.Instance.ChosenClass is Warrior) HolderClass.Instance.ChosenClass = (Warrior)HolderClass.Instance.ChosenClass;
        //else if (ChosenClass.ClassName == "Mage") ChosenClass = (Mage)ChosenClass;
        //else if (ChosenClass.ClassName == "Rogue") ChosenClass = (Rogue)ChosenClass;
        Labyrinth.CreateLabyrinth();
        Labyrinth.AddOptions();
        while (HolderClass.Instance.ChosenClass != null && HolderClass.Instance.ChosenClass.Health > 0)
        {
            SetUIState.DefaultSettings();
            HolderClass.Instance.IsMoving = true;
            PrintUI.Print();
            var coordinates = Labyrinth.GetCoordinates();
            if (!HolderClass.Instance.Rooms[coordinates.x][coordinates.y].IsFirstRoom && !HolderClass.Instance.Rooms[coordinates.x][coordinates.y].IsSolved)
            {
                HolderClass.Instance.Rooms[coordinates.x][coordinates.y].IsFirstRoom = false;
                HolderClass.Instance.SkipNextPrintOut = true;
                HolderClass.Instance.IsMoving = false;
                HolderClass.Instance.Rooms[coordinates.x][coordinates.y].CurrentEvent.Run();
            }
        }
        Console.WriteLine("Game Over");

    }
}