using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DungeonMaster.Classes;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using DungeonMaster.Descriptions;
using DungeonMaster.Other;
using System.IO.Pipes;


namespace DungeonMaster.Events
{
    public class StartGame : IEvent
    {
        #region PropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion

        public IEvent CurrentEvent { get; set; }
        public HolderClass HolderClass => HolderClass.Instance;
        private int _rerolls = 10;
        public string Type { get; } = "Start";
        public BaseClass ChosenClass { get; set; }
        public List<string> Description { get; set; }

        public StartGame()
        {
            HolderClass.HasOptionsHeader = true;
            HolderClass.OptionsHeader = "Welcome to Dungeon Master!";
            HolderClass.Instance.Options = new List<KeyValuePair<string, Action>>()
            {
                new KeyValuePair<string, Action>("1. Start new game", NameSelection),
                new KeyValuePair<string, Action>("2. Load game", () => { })
            };
            HolderClass.ShowOptions = true;
            PrintUI.Print();
        }



        private void NameSelection()
        {
            
            var namelist = new List<string>()
            {
                Names.RandomName(),
                Names.RandomName(),
                Names.RandomName(),
                Names.RandomName(),
                Names.RandomName(),
                Names.RandomName(),
            };
            HolderClass.Instance.Options = new List<KeyValuePair<string, Action>>()
            {
                new KeyValuePair<string, Action>($"1. {namelist[0]}", () => ClassSelection(namelist[0])),
                new KeyValuePair<string, Action>($"2. {namelist[1]}", () => ClassSelection(namelist[1])),
                new KeyValuePair<string, Action>($"3. {namelist[2]}", () => ClassSelection(namelist[2])),
                new KeyValuePair<string, Action>($"4. {namelist[3]}", () => ClassSelection(namelist[3])),
                new KeyValuePair<string, Action>($"5. {namelist[4]}", () => ClassSelection(namelist[4])),
                new KeyValuePair<string, Action>($"6. {namelist[5]}", () => ClassSelection(namelist[5]))
            };
            //PrintUI.Print();
        }


        private void ClassSelection(string name)
        {
            HolderClass.Instance.Options = new List<KeyValuePair<string, Action>>()
            {
                new KeyValuePair<string, Action>("1. Warrior", () => { HolderClass.Instance.ChosenClass = new Warrior(name, "Warrior"); HolderClass.Instance.ShowStats = true; RollStats(); }),
                new KeyValuePair<string, Action>("2. Ranger", () => { ChosenClass = new Warrior(name, "Ranger"); }),
                new KeyValuePair<string, Action>("3. Mage", () => { ChosenClass = new Warrior(name, "Mage"); })
            };
            //PrintUI.Print();

        }

        private void RollStats()
        {

            _rerolls--;
            HolderClass.Instance.ShowRolledStats = true;
            HolderClass.Instance.ShowStats = false;
            HolderClass.Instance.ChosenClass.RollStats();
            //ChosenClass.PrintRolledStats();

            HolderClass.Instance.Options = new List<KeyValuePair<string, Action>>()
            {
                new KeyValuePair<string, Action>("1. Yes", SetUIState),
                new KeyValuePair<string, Action>("2. No", RollStats ),

            };
            if (_rerolls > 0)
            {
                HolderClass.OptionsFooter = $"You have {_rerolls} rerolls left. Do you want to keep these stats?";
                //PrintUI.Print();
            }
        }

        public void SetUIState()
        {
            HolderClass.Instance.SkipNextPrintOut = true;
            HolderClass.Instance.ShowRolledStats = false;
            HolderClass.Instance.ShowStats = true;
            HolderClass.Instance.OptionsFooter = "Please select an option: ";
        }

        public void SetDefaultOptions()
        {
            throw new NotImplementedException();
        }

        public void Run()
        {
            throw new NotImplementedException();
        }

        public void BeforeNextRoom()
        {
            throw new NotImplementedException();
        }
    }
}
