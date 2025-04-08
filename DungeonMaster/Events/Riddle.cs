using DungeonMaster.Descriptions;
using DungeonMaster.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMaster.Events
{
    public class Riddle : IEvent
    {
        public string Type => "Riddle";
        public List<string> Description { get; set; }
        Random rnd = new Random();
        int r = 0;
        List<(string, string)> riddles = new List<(string riddle, string answer)>
        {
            ("I have no key, but I open a path, Not made of wood, nor iron’s wrath. Speak the truth, and I shall yield, Tell a lie, and remain sealed. What am I?", "door"),
            ("I am a portal to another world, I can take you to places unseen, but I require no magic spell to open. What am I?", "book"),
            ("I am a rare treasure, coveted by many, found in the depths, guarded by scales. Yet, I am not gold, nor am I silver. What am I?", "pearl"),
            ("I slumber in a cave of stone, a creature feared and yet unknown. With scales that gleam in the fire’s light and wings to soar to a great height. I breathe a flame, but not to kill, guarding treasure with my will. What am I?", "dragon"),
            ("The More That Is Here, The Less You Will See.", "darkness"),
            ("If Given One, You'll Have Many Or None.", "choice"),
            ("I Cross The River Without Moving. What Am I?", "bridge"),
            ("Time Existed Before Me, But History Can Only Begin After My Creation.", "writing"),
            ("Some Are Cherished, Some Are Hated, And Even If Lost, They Remain With You.", "memor"),
            ("They Arrive Every Night Whether Invited Or Not. They Can Be Seen, But Not Heard Or Touched. If One Falls, The Rest Keep Moving.", "star"),
            ("I Have Towns Without People, Forests Without Trees, And Rivers Without Water.", "map"),
            ("I Have A Tail And A Head, But No Legs. I Am Probably With You Now.", "coin"),
            ("I Am An Eye Set In A Blue Face. My Gaze Feeds The World. If I Go Blind, So Does The World.", "sun"),
            ("The More You Leave Behind, The More You Take.", "footstep"),
            ("What Breathes, Consumes, And Grows, But Was And Never Will Be Alive?", "fire"),
            ("I run smoother than most any rhyme; I love to fall but cannot climb.", "water"),
            ("What turns everything around but does not move?", "mirror"),
            ("Turn us on our backs and open our stomachs, you will be the wisest of men, though at start a lummox.", "book"),
            ("I'm rarely touched but often held. If you have wit, you'll use me well.", "tongue"),
            ("I know a word. Six letters it contains. Take away one. And twelve is what remains.", "dozen"),
            ("What does man love more than life? Fear more than death, or mortal strife? What the poor have, the rich require, and what contented men desire? What misers spend, and spendthrifts save, and all men carry to the grave?", "nothing"),
            ("The man who invented it doesn't want it. The man who bought it doesn't need it. The man who needs it doesn't know it.", "coffin")
        };

        public Riddle()
        {
            r = rnd.Next(riddles.Count);
            RoomDescription.GenerateRandomRoomDescription();
            RoomDescription.AddEventText($"{EventText()}", true);
            Description = RoomDescription.GetRandomRoomDescription();
        }

        private object EventText()
        {
            return $"You see a riddle on the wall. It reads: {riddles[r].Item1}";
        }

        public void BeforeNextRoom()
        {
            Labyrinth.SetRoomToSolved();
            HolderClass.Instance.SkipNextPrintOut = true;
            HolderClass.Instance.Options.Add(new KeyValuePair<string, Action>());
            HolderClass.Instance.Save();
        }

        public void Run()
        {
            SetUIState();
            SetDefaultOptions();
            PrintUI.Print();
            WriteAnswer();
        }

        private void WriteAnswer()
        {
            SetUIState();
            Console.SetCursorPosition(PrintUI.CursorX, PrintUI.CursorY);
            string text = Console.ReadLine();
            if (riddles[r].Item2.Contains(text.ToLower())) 
            {
                int x = rnd.Next(3);
                if (x == 0)
                {
                    HolderClass.Instance.ChosenClass.AltarDamageDoneModifier = 1.25;
                    PrintUI.SplitLog("You feel stronger. You will do more damage");
                }
                else if (x == 1)
                {
                    PrintUI.SplitLog("You feel stronger. Your strength, dexterity and intelligence have gone up.");
                    HolderClass.Instance.ChosenClass.AltarStrModifier = 1.25;
                    HolderClass.Instance.ChosenClass.AltarDexModifier = 1.25;
                    HolderClass.Instance.ChosenClass.AltarIntModifier = 1.25;
                }
                else
                {
                    PrintUI.SplitLog("You feel stronger. Your will be more likely to crit.");
                    HolderClass.Instance.ChosenClass.AltarCritModifier = 1.25;
                }
            }
            else
            {
                PrintUI.SplitLog("That wasn't correct. You flinch, expecting a punishment byt nothing happens");
                PrintUI.Print();
            }
            BeforeNextRoom();

        }

        public void SetDefaultOptions()
        {
            HolderClass.Instance.Options.Clear();
        }

        public void SetUIState()
        {
            HolderClass.Instance.SkipNextPrintOut = true;
            HolderClass.Instance.OptionsFooter = "What is the answer? ";
        }
    }
}
