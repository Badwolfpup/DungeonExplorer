using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml.Linq;
using DungeonMaster.Classes;
using DungeonMaster.Descriptions;
using DungeonMaster.Equipment;

namespace DungeonMaster.Events
{
    public class Battle : IEvent
    {
        #region PropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion

        private string _eventText;
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
        public List<KeyValuePair<string, Action>> Options { get; set; }
        private Monster _monster;
        private List<string> _monsternames = new List<string>()
        {
            "Goblin", "Orc", "Troll", "Dragon", "Skeleton", "Zombie", "Vampire", "Werewolf", "Ghost", "Lich", "Demon", "Gargoyle", 
            "Golem", "Kraken", "Beholder", "Mimic", "Chimera", "Griffon", "Basilisk", "Harpy"        
        }; 
        private List<string> _monsteradjectives = new List<string>()
        {
            "Angry", "Hungry", "Vicious", "Sneaky", "Sly", "Cunning", "Savage", "Ferocious", "Bloodthirsty", "Ravenous", "Fierce", 
            "Wild", "Cruel", "Brutal", "Merciless", "Vile", "Evil"
        };
        private List<string> _monsterprefix = new List<string>()
        {
            "ancient", "dark", "cursed", "blazing", "frozen", "mythic", "shadow", "holy", "demonic", "eldritch", "arcane", "infernal",
            "vnomous", "enchanted", "swift", "bloodthirsty", "stormforged", "ironclad", "dreadful", "titanic"
        };
        private List<string> _monstersuffix = new List<string>()
        {
            "of Doom", "of the Abyss", "of Shadows", "of the Fallen", "of Eternal Night", "of the Phoenix", "of the Lich", "of the Wilds",
            "of Frost", "of Infernos", "of the Arcane", "of Corruption", "of the Void", "of the Storm", "of Reckoning", "of the Titans",
            "of the Forgotten", "of the Cursed One", "of the Dragon", "of the Undying"
        };

        public Battle(BaseClass chosenClass)
        {
            ChosenClass = chosenClass;
            SetDefaultOptions();         
            EventText = RoomDescription.GetRandomRoomDescription();
            EventText += $"\nYou run into a {RandomizeMonster()}";           
            StartEvent();

        }

        private void StartEvent()
        {
            AddNewLine(2);
            while (_monster.Health > 0 && ChosenClass.Health > 0)
            {
                PrintOptions();                               
                TryChoice();
                AddNewLine(1);
            }
            AddNewLine(1);
            if (_monster.Health <= 0)
            {
                WonBattle();
            }
            else
            {
                EventText += "You have died.";
                
            }
        }

        private void WonBattle()
        {
            EventText = $"You have defeated the {_monster.Name}";
            if (_monster.Level < ChosenClass.Level - 3)
            {
                EventText = $"The monster level was too low for you to gain any experience points";
            } else
            {
                int newxp = _monster.Level <= ChosenClass.Level ? 100 - ((ChosenClass.Level - _monster.Level) * 2) : 10 + ((_monster.Level - ChosenClass.Level) * 3);
                ChosenClass.Experience += newxp;
                EventText = $"You have gained {newxp} experience points. ";
                AddNewLine(1);
                if (ChosenClass.Experience >= 100)
                {                  
                    EventText = ChosenClass.LevelUp(); 
                    AddNewLine(1);
                }
                EventText = $"You need {100-ChosenClass.Experience} xp to level up";
                AddNewLine(1);
            }
        }

        private void SetDefaultOptions()
        {
            Options = new List<KeyValuePair<string, Action>>()
            {
                new KeyValuePair<string, Action>("1. Attack", () => Attack()),
                new KeyValuePair<string, Action>("2. Use ability", () => UseAbility()),
                new KeyValuePair<string, Action>("3. Use item", () => UseItem()),
                new KeyValuePair<string, Action>("4. Run away", () => RunAway())
            };
            DefaultOptions.AddCharacterOptions(Options, ChosenClass);
            AddNewLine(2);
        }

        public void TryChoice()
        {
            string input = Console.ReadKey(true).KeyChar.ToString();
            if (Regex.IsMatch(input, $"^[1-{Options.Count}]$"))
            {
                EventText = input;
                AddNewLine(2);
                Options[int.Parse(input) - 1].Value();
            }
            else
            {
                TryChoice();
            }
        }

        private string RandomizeMonster()
        {
            Random r = new Random();
            string name = _monsternames[r.Next(_monsternames.Count)];
            string monstername = $"{_monsterprefix[r.Next(_monsterprefix.Count)]} {_monsteradjectives[r.Next(_monsteradjectives.Count)]} {name} {_monstersuffix[r.Next(_monstersuffix.Count)]}";
            _monster = new Monster(name, monstername);
            return monstername;
        }

        private void RunAway()
        {
            Random random = new Random();
            int escapechange = 30 + ChosenClass.Dexterity - (_monster.Level - ChosenClass.Level > 0 ? _monster.Level - ChosenClass.Level * 10 : 0);
            if (random.Next(100) < escapechange)
            {
                EventText = "You have successfully escaped";
                AddNewLine(1);
                EventText = "You run away from the monster";
            }
            else
            {
                EventText = "You have failed to escape";
                AddNewLine(1);
                EventText = "You stumble and fall to the ground unable to act for a moment";
                AddNewLine(1);
                
            }
        }

        private void UseItem()
        {
            Options = ChosenClass.PrintUseItems();
            Options.Add(new KeyValuePair<string, Action>($"{Options.Count + 1}. Go back to previous menu", () => SetDefaultOptions()));
        }

        private void UseAbility()
        {
            Options = ChosenClass.PrintSkillList();
            Options.Add(new KeyValuePair<string, Action>($"{Options.Count + 1}. Go back to previous menu", () => SetDefaultOptions()));
        }

        private void Attack()
        {
            double damage = Math.Round(ChosenClass.Attack()  * (1.0 - _monster.PhysicalDamageResist));
            _monster.Health -= (int)damage;  
            EventText = $"You swing your {ChosenClass.Equipment.FirstOrDefault(x => x is Weapon).Name} {(damage > 0 ? $"You deal {damage} damage. The {_monster.Name} has {_monster.Health} hp left" : $"You deal {damage} damage. The {_monster.Name} has died")}";
            AddNewLine(1);
        }


        public void RunEvent(int num)
        {
            Options[num].Value?.Invoke();
        }

        public void AddNewLine(int x)
        {
            EventText = "";
            EventText += new string('\n', x);
        }

        public void PrintOptions()
        {
            EventText = $"Your health: {ChosenClass.Health}/{ChosenClass.MaxHealth}  Your mana: {ChosenClass.Mana}/{ChosenClass.MaxMana}";
            AddNewLine(2);
            foreach (var s in Options)
            {
                EventText = s.Key + "\n";
            }
            EventText = "Please select an option: ";
            
        }

        public void AddDefaultOptions()
        {
            Options.Add(new KeyValuePair<string, Action>($"{Options.Count+1}. View character", () => DefaultOptions.AddCharacterOptions(Options, ChosenClass)));
        }
    }
}
