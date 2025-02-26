using DungeonMaster.Classes;
using DungeonMaster.Events;
using System;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;

namespace DungeonMaster;

class Program
{

    static void Main(string[] args)
    {
        BaseClass ChosenClass;
        StartGame start = new StartGame();
        ChosenClass = start.GetClass();
        if (ChosenClass is Warrior) ChosenClass = (Warrior)ChosenClass;
        //else if (ChosenClass.ClassName == "Mage") ChosenClass = (Mage)ChosenClass;
        //else if (ChosenClass.ClassName == "Rogue") ChosenClass = (Rogue)ChosenClass;
        IEvent CurrentEvent;
        while (ChosenClass != null && ChosenClass.Health > 0)
        {
            Random rnd = new Random();
            int random = rnd.Next(100);
            CurrentEvent = random switch
            {
                >= 0 => new Battle(ChosenClass),
            };
        }
        Console.WriteLine("Game Over");


    }
    
}