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
    /// <summary>
    /// Interface for all events.
    /// </summary>
    public interface IEvent
    {
        string Type { get; } // Type of event
        List<string> Description { get; set; } // Description of the event
        void SetUIState(); // Set the UI state for the event - which setting to be toggled
        void SetDefaultOptions(); // Set the default options for the event
        void Run(); // The method the executes the event
        void BeforeNextRoom(); // The method that is called before moving on to the next room

    }
}
