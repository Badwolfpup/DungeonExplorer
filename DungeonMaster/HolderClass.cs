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
        public static HolderClass Instance
        {
            get
            {
                try
                {
                    return _instance ??= new HolderClass();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception in HolderClass.Instance: {ex.Message}");
                    throw;
                }
            }
        }

        public int FloorLevel { get; set; } = 1;
        public int Turn { get; set; } = 1;
        public BaseClass ChosenClass;
        public BaseClass Monster;
        public bool PlayerUsedAction { get; set; } = true;
        public (int x, int y)CharacterPane { get; set; } = (1, 1); 
        public (int x, int y) Statspane { get; set; } = (1, 8);
        public (int x, int y) EquipmentPane { get; set; } = (1, 14);
        public (int x, int y) ItemPane { get; set; } = (1, 20);
        public (int x, int y) DescPane { get; set; } = (30, 1);
        public (int x, int y) OptionsPane { get; set; } = (30, 11);
        public (int x, int y) MapPane { get; set; } = (30, 21);
        public (int x, int y) MonstwePane { get; set; } = (90, 1);
        public (int x, int y) LogPane { get; set; } = (90, 8);
        public (int x, int y) CurrentPosition { get; set; } = (0, 0);
        public List<KeyValuePair<string, Action>> Options { get; set; } = new List<KeyValuePair<string, Action>>();
        public List<KeyValuePair<string, Action<string>>> DirectionalOptions { get; set; } = new List<KeyValuePair<string, Action<string>>>();
        public List<List<Room>> Rooms { get; set; }
        public string OptionsHeader { get; set; }
        public string OptionsFooter { get; set; } = "Please select an option: ";
        public bool SkipNextPrintOut { get; set; }
        public bool SkipNextTryChoice { get; set; }

        public bool HasOptionsHeader { get; set; }
        public bool HasMonster { get; set; }
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
        public bool IsPlayerTurn { get; set; } = true;
        public bool IsMoving { get; set; }

        private HolderClass()
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in HolderClass constructor: {ex.Message}");
                throw;
            }
        }


    }
}
