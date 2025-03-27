using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMaster.Other
{
    public static class SetUIState
    {
        public static void BeforeMoving()
        {
            HolderClass.Instance.OptionsFooter = "Please select an option: ";
            HolderClass.Instance.SkipNextPrintOut = false;
            HolderClass.Instance.HasOptionsHeader = false;
            HolderClass.Instance.ShowCharacter = true;
            HolderClass.Instance.ShowEquipment = true;
            HolderClass.Instance.ShowItems = true;
            HolderClass.Instance.ShowDescription = true;
            HolderClass.Instance.ShowOptions = true;
            HolderClass.Instance.ShowMap = true;
            HolderClass.Instance.ShowLog = false;
            HolderClass.Instance.ShowMonster = false;
            HolderClass.Instance.ShowStats = true;
            HolderClass.Instance.ShowRolledStats = false;
        }

        public static void DefaultSettings()
        {
            HolderClass.Instance.OptionsFooter = "Please select an option: ";
            HolderClass.Instance.SkipNextPrintOut = false;
            HolderClass.Instance.SkipNextTryChoice = false;
            HolderClass.Instance.HasOptionsHeader = false;
            HolderClass.Instance.ShowCharacter = true;
            HolderClass.Instance.ShowEquipment = true;
            HolderClass.Instance.ShowItems = true;
            HolderClass.Instance.ShowDescription = true;
            HolderClass.Instance.ShowOptions = true;
            HolderClass.Instance.ShowMap = true;
            HolderClass.Instance.ShowLog = true;
            HolderClass.Instance.ShowMonster = false;
            HolderClass.Instance.ShowStats = true;
            HolderClass.Instance.ShowRolledStats = false;
        }
    }
}
