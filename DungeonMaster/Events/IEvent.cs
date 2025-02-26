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
    public interface IEvent: INotifyPropertyChanged
    {
        List<KeyValuePair<string, Action>> Options { get; set; }
        string EventText { get; set; }
        //IEvent CurrentEvent { get; set; }
        //IBaseClass ChosenClass { get; set; }
        void RunEvent(int num);

        void TryChoice();
        
        void AddNewLine(int x);

        void PrintOptions();

        void AddDefaultOptions();

        
    }
}
