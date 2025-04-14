using DungeonMaster.Classes;
using DungeonMaster.Descriptions;
using DungeonMaster.Events;
using DungeonMaster.Other;
using System;
using System.ComponentModel.Design;
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
        SaveLoad savefile = new SaveLoad();
        while (true)
        {
            StartGame start = new StartGame();
            if (HolderClass.Instance.ChosenClass is Warrior) HolderClass.Instance.ChosenClass = (Warrior)HolderClass.Instance.ChosenClass;
            //else if (ChosenClass.ClassName == "Mage") ChosenClass = (Mage)ChosenClass;
            //else if (ChosenClass.ClassName == "Rogue") ChosenClass = (Rogue)ChosenClass;
            Labyrinth.AddOptions();
            while (HolderClass.Instance.ChosenClass != null && HolderClass.Instance.ChosenClass.Health > 0)
            {
                if (HolderClass.Instance.IsNewFloor) Labyrinth.CreateLabyrinth();
                HolderClass.Instance.IsNewFloor = false;
                SetUIState.DefaultSettings();
                //if (!HolderClass.Instance.HasTeleported) 
                HolderClass.Instance.IsMoving = true;
                PrintUI.Print();
                var coordinates = Labyrinth.GetCoordinates();
                if (!HolderClass.Instance.Rooms[coordinates.x][coordinates.y].IsFirstRoom && !HolderClass.Instance.Rooms[coordinates.x][coordinates.y].IsSolved)
                {
                    if (HolderClass.Instance.Rooms[coordinates.x][coordinates.y].CurrentEvent is Boss && !HolderClass.Instance.HasMoved && HolderClass.Instance.HasEnteredBossRoom)
                    {

                    } else if (HolderClass.Instance.Rooms[coordinates.x][coordinates.y].CurrentEvent is Stairs && !HolderClass.Instance.HasMoved && HolderClass.Instance.HasEnteredStairs)
                    {

                    }
                    else
                    {
                        HolderClass.Instance.Rooms[coordinates.x][coordinates.y].IsFirstRoom = false;
                        HolderClass.Instance.SkipNextPrintOut = true;
                        HolderClass.Instance.IsMoving = false;
                        HolderClass.Instance.HasTeleported = false;
                        HolderClass.Instance.Rooms[coordinates.x][coordinates.y].CurrentEvent.Run();
                    }
                }
                savefile.Save();
            }
            HolderClass.Instance.SkipNextPrintOut = false;
            HolderClass.Instance.ShowStats = false;
            Console.Clear();
            string[] gameOver = {
                "  ██████╗  █████╗ ███╗   ███╗███████╗     ██████╗ ██╗   ██╗███████╗██████╗ ",
                " ██╔════╝ ██╔══██╗████╗ ████║██╔════╝    ██╔═══██╗██║   ██║██╔════╝██╔══██╗",
                " ██║  ███╗███████║██╔████╔██║█████╗      ██║   ██║██║   ██║█████╗  ██████╔╝",
                " ██║   ██║██╔══██║██║╚██╔╝██║██╔══╝      ██║▄▄ ██║██║   ██║██╔══╝  ██╔══██╗",
                " ╚██████╔╝██║  ██║██║ ╚═╝ ██║███████╗    ╚██████╔╝╚██████╔╝███████╗██║  ██║",
                "  ╚═════╝ ╚═╝  ╚═╝╚═╝     ╚═╝╚══════╝     ╚══▀▀═╝  ╚═════╝ ╚══════╝╚═╝  ╚═╝"
            };

            int centerX = (Console.WindowWidth - gameOver[0].Length) / 2;
            int centerY = (Console.WindowHeight - gameOver.Length) / 2;

            for (int i = 0; i < gameOver.Length; i++)
            {
                Console.SetCursorPosition(centerX, centerY + i);
                Console.Write(gameOver[i]);
                Thread.Sleep(200);
            }

            Console.SetCursorPosition((Console.WindowWidth - 23) / 2, centerY + 8);
            Console.Write("Press any key to continue...");
            Console.ReadKey();
        }


    }
}