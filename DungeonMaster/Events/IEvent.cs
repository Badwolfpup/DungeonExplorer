using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DungeonMaster.Classes;

namespace DungeonMaster.Events
{
    public interface IEvent
    {
        //List<KeyValuePair<string, Action>> Options { get; set; }
        //IEvent CurrentEvent { get; set; }
        //IBaseClass ChosenClass { get; set; }


        //void TryChoice();

        //void AddNewLine(int x);

        //void PrintOptions();
        string Type { get; }
        List<string> Description { get; set; }
        void SetUIState();
        void SetDefaultOptions();
        void Run();
        void BeforeNextRoom();
        
    }
}
