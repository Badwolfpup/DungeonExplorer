using DungeonMaster.Classes;
using DungeonMaster.Skills;
using DungeonMaster.Skills.Melee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Newtonsoft.Json;
using DungeonMaster.Other;

namespace DungeonMaster
{
    public class HolderClass
    {
        private static HolderClass _instance;
        public static HolderClass Instance = _instance ??= new HolderClass();
        private HolderClass()
        {

        }

        public int FloorLevel { get; set; } = 0; //Keeps track of the current floor level
        public int Turn { get; set; } = 1; //Keeps track of the current turn
        public BaseClass ChosenClass; //The player character
        public BaseClass Monster; //The current monster

        //Used to set the coordinates when printing the UI
        public (int x, int y) CharacterPane { get; set; } = (1, 1); 
        public (int x, int y) Statspane { get; set; } = (1, 8);
        public (int x, int y) EquipmentPane { get; set; } = (1, 14);
        public (int x, int y) ItemPane { get; set; } = (1, 20);
        public (int x, int y) DescPane { get; set; } = (30, 1);
        public (int x, int y) OptionsPane { get; set; } = (30, 11);
        public (int x, int y) MapPane { get; set; } = (30, 21);
        public (int x, int y) MonstwePane { get; set; } = (90, 1);
        public (int x, int y) LogPane { get; set; } = (90, 8);
        public (int x, int y) CurrentPosition { get; set; } = (0, 0);

        public List<KeyValuePair<string, Action>> Options { get; set; } = new List<KeyValuePair<string, Action>>(); //The list holds the options for the player to choose from
        public List<KeyValuePair<string, Action<string>>> DirectionalOptions { get; set; } = new List<KeyValuePair<string, Action<string>>>(); //Used for movement options
        public List<List<Room>> Rooms { get; set; } //The list of rooms in the labyrinth
        public string OptionsHeader { get; set; } //The header for the options pane
        public string OptionsFooter { get; set; } = "Please select an option: "; //The footer for the options pane
        public bool PlayerUsedAction { get; set; } = true; //Keeps track of whether the player has used their action
        public bool SkipNextPrintOut { get; set; } //Used to skip the next printout
        public bool SkipNextTryChoice { get; set; } //You print the UI but skip the next try choice
        public bool IsNewFloor { get; set; } = true; //Keeps track of whether the player is on a new floor
        public bool HasOptionsHeader { get; set; } //Used to check if the options header should be printed
        public bool HasMonster { get; set; } //Used to check if there is a monster in the room

        //These are used to determine which part of the UI should be printed
        public bool ShowRolledStats { get; set; }
        public bool ShowStats { get; set; }
        public bool ShowCharacter { get; set; } 
        public bool ShowEquipment { get; set; }
        public bool ShowItems { get; set; }
        public bool ShowDescription { get; set; }
        public bool ShowOptions { get; set; }
        public bool ShowMap { get; set; }
        public bool ShowMonster { get; set; }
        public bool ShowLog { get; set; }

        public bool IsPlayerTurn { get; set; } = true; //Used to check if it is the player's or monster's turn
        public bool IsMoving { get; set; } //Used to determine whether to display movement option or regular options
        public bool IsBossFight { get; set; } //Used to determine if the current room is a boss fight
        public bool HasTeleported { get; set; } //Used to determine if the player has just teleported
        public bool HasStairs { get; set; } //Used to determine if the the room is a stairs room
        public bool IsBossDead { get; set; } //Used to determine if the boss has been killed on the current floor
        public bool HasMoved { get; set; }  //Used to determine if the player has moved in the current turn and therefore should print the room
        public bool HasEnteredBossRoom { get; set; } //Used to determine if the player is in the boss room
        public bool HasEnteredStairs { get; set; } //Used to determine if the player is in the stairs room
       

 

    }
}
