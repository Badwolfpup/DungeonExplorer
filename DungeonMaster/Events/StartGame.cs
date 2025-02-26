using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DungeonMaster.Classes;
using FantasyGameNameGenerator;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using System.Text.RegularExpressions;


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

        public List<KeyValuePair<string, Action>> Options { get; set; }

        public IEvent CurrentEvent { get; set; }
        private string _eventText;
        private int _rerolls = 10;

        public string EventText
        {
            get => _eventText;
            set
            {
                if (_eventText != value)
                {
                    _eventText = value;
                    Console.Write(_eventText);
                }
            }
        }
        public BaseClass ChosenClass { get; set; }

        public StartGame()
        {

            Options = new List<KeyValuePair<string, Action>>()
            {
                new KeyValuePair<string, Action>("1. Start new game", () => NameSelection()),
                new KeyValuePair<string, Action>("2. Load game", () => { })
            };
            EventText = "Welcome to the Text Adventure Game!\n";
            PrintOptions();
            EventText = "Please select an option: ";
            TryChoice();
        }

        public void PrintOptions()
        {
            foreach (var s in Options)
            {
                EventText = s.Key + "\n";
            }
        }

        public void TryChoice()
        {
            string input = Console.ReadKey(true).KeyChar.ToString();
            if (Regex.IsMatch(input, $"^[1-{Options.Count}]$")) {
                EventText = input;
                Options[int.Parse(input) - 1].Value();
            } else
            {
                TryChoice();
            }
        }

        private void NameSelection()
        {
            var name = new NameGenerator();
            var namelist = new List<string>()
            {
                name.GenerateName(),
                name.GenerateName(),
                name.GenerateName(),
                name.GenerateName(),
                name.GenerateName(),
                name.GenerateName()
            };
            Options = new List<KeyValuePair<string, Action>>()
            {
                new KeyValuePair<string, Action>($"1. {namelist[0]}", () => ClassSelection(namelist[0])),
                new KeyValuePair<string, Action>($"2. {namelist[1]}", () => ClassSelection(namelist[1])),
                new KeyValuePair<string, Action>($"3. {namelist[2]}", () => ClassSelection(namelist[2])),
                new KeyValuePair<string, Action>($"4. {namelist[3]}", () => ClassSelection(namelist[3])),
                new KeyValuePair<string, Action>($"5. {namelist[4]}", () => ClassSelection(namelist[4])),
                new KeyValuePair<string, Action>($"6. {namelist[5]}", () => ClassSelection(namelist[5]))
            };
            AddNewLine(2);
            PrintOptions();
            EventText = "Please choose your name: ";
            TryChoice();
        }

        public void AddNewLine(int x)
        {
            EventText = "";
            EventText += new string('\n', x);
        }

        private void ClassSelection(string name)
        {
            Options = new List<KeyValuePair<string, Action>>()
            {
                new KeyValuePair<string, Action>("1. Warrior", () => { ChosenClass = new Warrior(name, "Warrior");  RollStats(); }),
                new KeyValuePair<string, Action>("2. Ranger", () => { ChosenClass = new Warrior(name, "Ranger"); }),
                new KeyValuePair<string, Action>("3. Mage", () => { ChosenClass = new Warrior(name, "Mage"); })
            };
            AddNewLine(2);
            PrintOptions();
            EventText = "Please select your class: ";
            TryChoice();
        }

        private void RollStats()
        {
            AddNewLine(2);
            _rerolls--;
            ChosenClass.RollStats();
            //AddNewLine(1);
            ChosenClass.PrintRolledStats();
            AddNewLine(1);
            Options = new List<KeyValuePair<string, Action>>()
            {
                new KeyValuePair<string, Action>("1. Yes", () => {  }),
                new KeyValuePair<string, Action>("2. No", () => { RollStats(); }),

            };
            AddNewLine(1);
            PrintOptions();

            EventText = _rerolls > 0 ? $"Are you happy with your stats? {_rerolls} rerolls remaining. " : "";
            TryChoice();
            AddNewLine(2);
        }

        public void RunEvent(int num)
        {
            Options[num].Value?.Invoke();
        }

        public BaseClass GetClass() => ChosenClass;

        public void AddDefaultOptions()
        {
            throw new NotImplementedException();
        }
    }
}
