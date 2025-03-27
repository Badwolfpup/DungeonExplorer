using DungeonMaster.Classes;
using DungeonMaster.FormatClass;
using DungeonMaster.Other;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DungeonMaster.Descriptions
{
    static class PrintUI
    {
        public static StringBuilder FullUI { get; set; } = new StringBuilder();
        public static List<string> desc { get; set; } = new List<string>();
        static bool HasPrintedOption = false;
        static bool HasPrintedRolledStat = false;
        static bool HasPrintedDescription = false;
        static bool HasPrintedCharacter = false;
        static bool HasPrintedStats = false;
        static bool HasPrintedEquipment = false;
        static bool HasPrintedItems = false;
        static bool HasPrintedMap = false;
        static bool HasPrintedLog = false;
        static bool HasPrintedMonster = false;
        static int CursorX { get; set; }
        static int CursorY { get; set; }
        static int Optionscount = 0;
        static int OriginalCount;
        static int PrintedOptions = 0;
        static PrintUI()
        {
            
        }

        
        public static List<string> CombatLog { get; set; } = new List<string>();


        private static void TryChoice()
        {
            if (HolderClass.Instance.SkipNextPrintOut)
            {
                HolderClass.Instance.SkipNextPrintOut = false;
                return;
            }
            var coordinates = Labyrinth.GetCoordinates();
            if (HolderClass.Instance.Rooms != null && HolderClass.Instance.Rooms[coordinates.x][coordinates.y].IsFirstRoom) HolderClass.Instance.SkipNextPrintOut = true;

            if (HolderClass.Instance.Options.Count == 0) return;
            Console.SetCursorPosition(CursorX, CursorY);
            string input = Console.ReadKey(true).KeyChar.ToString();
            if (HolderClass.Instance.IsMoving)
            {
                if (Regex.IsMatch(input, $"^[1-{HolderClass.Instance.DirectionalOptions.Count}]$"))
                {
                    Console.Write(input);
                    string text = input switch
                    {
                        "1" => "Left",
                        "2" => "Up",
                        "3" => "Right",
                        "4" => "Down"
                    };
                    ExecuteClass.ExecuteDirections(HolderClass.Instance.DirectionalOptions[int.Parse(input) - 1].Value, text);
                }
                else
                {
                    TryChoice();
                }
            }
            else
            {
                if (Regex.IsMatch(input, $"^[1-{HolderClass.Instance.Options.Count}]$"))
                {
                    Console.Write(input);
                    ExecuteClass.Execute(HolderClass.Instance.Options[int.Parse(input) - 1].Value);
                }
                else
                {
                    TryChoice();
                }
            }
        }

        public static void Print()
        {
            
            FullUI.Clear();
            if (HolderClass.Instance.IsMoving) Optionscount = HolderClass.Instance.DirectionalOptions.Count;
            else Optionscount = HolderClass.Instance.Options.Count;
            OriginalCount = Optionscount;
            PrintedOptions = 0;
            (string, bool) result;
            for (int j = 0; j < 36; j++)
            {
                HasPrintedOption = false;
                HasPrintedRolledStat = false;
                HasPrintedDescription = false;
                HasPrintedCharacter = false;
                HasPrintedStats = false;
                HasPrintedEquipment = false;
                HasPrintedLog = false;
                HasPrintedItems = false;
                HasPrintedMonster = false;
                for (int i = 0; i < 150; i++)
                {
                    result = IsCornerCharacter(i, j); if (result.Item2) { FullUI.Append(result.Item1); continue; } 
                    result = IsTCharacter(i, j); if (result.Item2) { FullUI.Append(result.Item1); continue; }
                    result = IsCrossCharacter(i, j); if (result.Item2) { FullUI.Append(result.Item1); continue; }
                    result = IsVerticalCharacter(i, j); if (result.Item2) { FullUI.Append(result.Item1); continue; }
                    result = IsHorizontalCharacter(i, j); if (result.Item2) { FullUI.Append(result.Item1); continue; }
                    if (HolderClass.Instance.ShowOptions) result = PrintOptionsPane(i, j); if (result.Item2) { FullUI.Append(result.Item1 + string.Concat(Enumerable.Repeat(" ", 59 - result.Item1.Length))); i = 89; continue; }
                    if (HolderClass.Instance.ShowRolledStats) result = PrintRolledStats(i, j); if (result.Item2) { FullUI.Append(result.Item1 + string.Concat(Enumerable.Repeat(" ", 29 - result.Item1.Length))); i = 29; continue; }
                    if (HolderClass.Instance.ShowDescription) result = PrintDescriptionPane(i, j); if (result.Item2) { FullUI.Append(result.Item1 + string.Concat(Enumerable.Repeat(" ", 59 - result.Item1.Length))); i = 89; continue; }
                    if (HolderClass.Instance.ShowCharacter) result = PrintCharacterPane(i, j); if (result.Item2) { FullUI.Append(result.Item1 + string.Concat(Enumerable.Repeat(" ", 29 - result.Item1.Length))); i = 29; continue; }
                    if (HolderClass.Instance.ShowStats) result = PrintStatsPane(i, j); if (result.Item2) { FullUI.Append(result.Item1 + string.Concat(Enumerable.Repeat(" ", 29 - result.Item1.Length))); i = 29; continue; }
                    if (HolderClass.Instance.ShowEquipment) result = PrintEquipmentPane(i, j); if (result.Item2) { FullUI.Append(result.Item1 + string.Concat(Enumerable.Repeat(" ", 29 - result.Item1.Length))); i = 29; continue; }
                    if (HolderClass.Instance.ShowItems) result = PrintItems(i, j); if (result.Item2) { FullUI.Append(result.Item1 + string.Concat(Enumerable.Repeat(" ", 29 - result.Item1.Length))); i = 29; continue; }
                    //if (HolderClass.Instance.ShowMap) result = PrintMap(i, j); if (result.Item2) { FullUI.Append(string.Concat(Enumerable.Repeat(" ", 30-Labyrinth.numberX/2)) + result.Item1 + string.Concat(Enumerable.Repeat(" ", 29 - Labyrinth.numberX/2))); i = 89; continue; }
                    if (HolderClass.Instance.ShowMap) result = PrintMap(i, j); if (result.Item2) { FullUI.Append(string.Concat(Enumerable.Repeat(" ", 29 - result.Item1.Length / 2)) + result.Item1 + string.Concat(Enumerable.Repeat(" ", 29 - result.Item1.Length / 2))); i = 89; continue; }
                    if (HolderClass.Instance.ShowLog) result = PrintLog(i, j); if (result.Item2) { FullUI.Append(result.Item1 + string.Concat(Enumerable.Repeat(" ", 58 - result.Item1.Length))); i = 148; continue; }
                    if (HolderClass.Instance.ShowMonster) result = PrintMonster(i, j); if (result.Item2) { FullUI.Append(result.Item1 + string.Concat(Enumerable.Repeat(" ", 58 - result.Item1.Length))); i = 148; continue; }
                    if (!HasPrintedOption || !HasPrintedRolledStat || !HasPrintedDescription || !HasPrintedItems || !HasPrintedMonster || !HasPrintedEquipment) FullUI.Append(' ');
                }
                if (j< 29) FullUI.Append("\n");
            }
            Console.SetCursorPosition(0, 0);
            Console.Write(FullUI.ToString());
            ResetParameters();
            if (HolderClass.Instance.IsPlayerTurn && !HolderClass.Instance.SkipNextTryChoice) TryChoice();

        }

        private static void AddTextToSB(string text)
        {
            var list = FullUI.ToString().Split(" ").ToList();
            if (list[list.Count - 1] == "Health:" || list[list.Count - 1] == "Mana:")
            {
                Console.ForegroundColor = TextColor.HealthMana(HolderClass.Instance.ChosenClass);
                FullUI.Append(text + string.Concat(Enumerable.Repeat(" ", 29 - text.Length)));
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }
            FullUI.Append(text + string.Concat(Enumerable.Repeat(" ", 29 - text.Length)));
        }

        private static void ResetParameters()
        {
            
        }

        #region PrintPanes
        public static (string,bool) PrintOptionsPane(int x, int y)
        {
            if (HasPrintedOption) return ("", false);
            int StartX = 31;
            int StartY = 13;
            int EndX = 89;
            int EndY = 23; 
            if (x >= StartX && x <= EndX && y >= StartY && y <= EndY)
            {
                if (y == 11 && HolderClass.Instance.HasOptionsHeader)
                {
                    HasPrintedOption = true;
                    return (HolderClass.Instance.OptionsHeader, true);
                } 
                else
                {
                    //int index = y - StartY;
                    if (Optionscount >0)
                    {
                        string text = "";
                        if (HolderClass.Instance.IsMoving) text = HolderClass.Instance.DirectionalOptions[PrintedOptions].Key;
                        else text = HolderClass.Instance.Options[PrintedOptions].Key;
                        Optionscount--;
                        PrintedOptions++;
                        //HolderClass.Instance.Options.RemoveAt(0);
                        HasPrintedOption = true;
                        return (text, true);
                    }
                }
                int lastline = 13 + OriginalCount + (HolderClass.Instance.HasOptionsHeader ? 1 : 0);
                if (y == lastline)
                {
                    HasPrintedOption = true;
                    string text = "";
                    text = HolderClass.Instance.OptionsFooter;
                    CursorY = lastline;
                    CursorX = StartX + text.Length;
                    return (text, true);

                }
            }
            return ("", false);
        }

        public static (string, bool) PrintRolledStats(int x, int y)
        {
            if (HasPrintedRolledStat) return ("", false); 
            int StartX = 1;
            int StartY = 1;
            int EndX = 29;
            int EndY = 7;
            desc = HolderClass.Instance.ChosenClass.PrintRolledStats();
            if (x >= StartX && x <= EndX && y >= StartY && y <= EndY)
            {
                int index = y - StartY;
                if (index < desc.Count)
                {
                    HasPrintedRolledStat = true;
                    return (desc[index], true);

                }
            }
            return ("", false);
        }

        public static (string, bool) PrintCharacterPane(int x, int y)
        {
            if (HasPrintedCharacter) return ("", false);
            int StartX = 1;
            int StartY = 1;
            int EndX = 29;
            int EndY = 7;
            desc = HolderClass.Instance.ChosenClass.PrintCharacter();
            if (x >= StartX && x <= EndX && y >= StartY && y <= EndY)
            {
                int index = y - StartY;
                if (index < desc.Count)
                {
                    HasPrintedCharacter = true;
                    return (desc[index], true);

                }
            }
            return ("", false);
        }

        private static (string, bool) PrintStatsPane(int x, int y)
        {
            if (HasPrintedStats) return ("", false);
            int StartX = 1;
            int StartY = 9;
            int EndX = 29;
            int EndY = 15;
            desc = HolderClass.Instance.ChosenClass.PrintStats();
            if (x >= StartX && x <= EndX && y >= StartY && y <= EndY)
            {
                int index = y - StartY;
                if (index < desc.Count)
                {
                    HasPrintedStats = true;
                    return (desc[index], true);

                }
            }
            return ("", false);
        }

        public static (string, bool) PrintDescriptionPane(int x, int y)
        {
            if (HasPrintedDescription) return ("", false);
            int StartX = 31;
            int StartY = 1;
            int EndX = 89;
            int EndY = 11;
            var coordinates = Labyrinth.GetCoordinates();
            desc = HolderClass.Instance.Rooms[coordinates.x][coordinates.y].CurrentEvent.Description;

            if (x >= StartX && x <= EndX && y >= StartY && y <= EndY)
            {
                int index = y - StartY;
                if (index < desc.Count)
                {
                    HasPrintedDescription = true;
                    return (desc[index], true);

                }

            }

            return ("", false);
        }

        private static (string, bool) PrintEquipmentPane(int x, int y)
        {
            if (HasPrintedEquipment) return ("", false);
            int StartX = 1;
            int StartY = 17;
            int EndX = 29;
            int EndY = 23;
            desc = HolderClass.Instance.ChosenClass.PrintEquipment();
            if (x >= StartX && x <= EndX && y >= StartY && y <= EndY)
            {
                int index = y - StartY;
                if (index < desc.Count)
                {
                    HasPrintedEquipment = true;
                    return (desc[index], true);

                }
            }
            return ("", false);
        }

        public static (string, bool) PrintItems(int x, int y)
        {
            if (HasPrintedItems) return ("", false);
            int StartX = 1;
            int StartY = 25;
            int EndX = 29;
            int EndY = 34;
            desc = HolderClass.Instance.ChosenClass.PrintBag();
            if (x >= StartX && x <= EndX && y >= StartY && y <= EndY)
            {
                int index = y - StartY;
                if (index < desc.Count)
                {
                    HasPrintedItems = true;
                    return (desc[index], true);

                }
            }
            return ("", false);
        }

        private static (string, bool) PrintMap(int x, int y)
        {
            if (HasPrintedMap) return ("", false);
            int StartX = 31;
            int StartY = 25;
            int EndX = 89;
            int EndY = 34;
            desc = Labyrinth.PrintMap();
            if (StartY == y && x >= StartX && x < EndX)
            {
                return ($"\u2584{string.Concat(Enumerable.Repeat("\u2584", 15))}\u2584", true);
            } 
            if (y == EndY && x >= StartX && x < EndX)
            {
                return ($"\u2580{string.Concat(Enumerable.Repeat("\u2580", 15))}\u2580", true);
            }
            if (x >= StartX && x <= EndX && y > StartY && y < EndY)
            {
                int index = y - StartY - 1;
                if (index < desc.Count)
                {
                    HasPrintedItems = true;
                    return ($"\u258C{desc[index]}\u2590", true);

                }
            }
                return ("", false);
        }

        public static (string, bool) PrintLog(int x, int y)
        {
            if (HasPrintedLog) return ("", false);
            int StartX = 91;
            int StartY = 9;
            int EndX = 148;
            int EndY = 34;
            //desc = HolderClass.Instance.Monster.PrintMonster();
            if (x >= StartX && x <= EndX && y >= StartY && y <= EndY)
            {
                int index = y - StartY;

                if (index < CombatLog.Count)
                {
                    HasPrintedLog = true;
                    return (CombatLog[index], true);

                }
            }
            return ("", false);
        }
        

        private static (string, bool) PrintMonster(int x, int y)
        {
            if (HasPrintedMonster) return ("", false);
            int StartX = 91;
            int StartY = 1;
            int EndX = 148;
            int EndY = 7;
            desc = HolderClass.Instance.Monster.PrintMonster();
            if (x >= StartX && x <= EndX && y >= StartY && y <= EndY)
            {
                int index = y - StartY;

                if (index < desc.Count)
                {
                    HasPrintedMonster = true;
                    return (desc[index], true);

                }
            }
            return ("", false);
        }
        #endregion

        public static void SplitLog(string text)
        {
            List<string> desc = text.Split(" ").ToList();
            string currentLine = "";
            int lineLength = 58;
            while (desc.Count > 0)
            {
                if (desc[0].Length + 1 < lineLength)
                {
                    currentLine += $"{(lineLength != 58 ? " " : "")}{desc[0]}";
                    lineLength -= desc[0].Length + 1;
                    desc.RemoveAt(0);
                }
                else
                {
                    CombatLog.Add(currentLine);
                    if (CombatLog.Count > 27) CombatLog.RemoveAt(0);
                    currentLine = "";
                    lineLength = 58;
                }

            }
            CombatLog.Add(currentLine);
            if (CombatLog.Count > 27) CombatLog.RemoveAt(0);
        }

        public static void SplitLog(string text, List<(string, ConsoleColor)> highlights)
        {
            List<string> desc = text.Split(" ").ToList();
            string currentLine = "";
            int lineLength = 58;
            while (desc.Count > 0)
            {
                if (desc[0].Length + 1 < lineLength)
                {
                    currentLine += $"{(lineLength != 58 ? " " : "")}{desc[0]}";
                    lineLength -= desc[0].Length + 1;
                    desc.RemoveAt(0);
                }
                else
                {
                    CombatLog.Add(currentLine);
                    if (CombatLog.Count > 27) CombatLog.RemoveAt(0);
                    currentLine = "";
                    lineLength = 58;
                }

            }
            CombatLog.Add(currentLine);
            if (CombatLog.Count > 27) CombatLog.RemoveAt(0);
        }





        #region PrintBorders


        private static (string, bool) IsCornerCharacter(int posX, int posY)
        {
            if (posX == 0 && posY == 0) { return ("\u250C", true); } //┌
            if (posX == 149 && posY == 0) return ("\u2510", true); //┐
            if (posX == 0 && posY == 35) return ("\u2514", true); //└
            if (posX == 149 && posY == 35) return ("\u2518", true); //┘
            return ("", false);
        }

        private static (string, bool) IsTCharacter(int posX, int posY)
        {
            if ((posX == 30 && posY == 0) || (posX == 90 && posY == 0)) return ("\u252C", true); //┬
            if ((posX == 30 && posY == 35) || (posX == 90 && posY == 35)) return ("\u2534", true); //┴
            if ((posX == 0 && posY == 8) || (posX == 0 && posY == 16) || (posX == 0 && posY == 24) || (posX == 30 && posY == 12) || (posX == 90 && posY == 8)) return ("\u251C", true); //├
            if ((posX == 30 && posY == 8) || (posX == 30 && posY == 16) || (posX == 90 && posY == 12) || (posX == 90 && posY == 24) || (posX == 149 && posY == 8)) return ("\u2524", true); //┤
            return ("", false);
        }

        private static (string, bool) IsCrossCharacter(int posX, int posY)
        {
            if (posX == 30 && posY == 24) return ("\u253C", true);
            return ("", false);
        }

        private static (string, bool) IsVerticalCharacter(int posX, int posY)
        {
            if (posX == 0 || posX == 30 || posX == 90 || posX == 149) return ("\u2502", true); //│
            return ("", false);
        }

        private static (string, bool) IsHorizontalCharacter(int posX, int posY)
        {
            if (posY == 0  || (posY == 8 && posX < 30) || (posY == 8 && posX > 90) || 
               (posY == 12 && posX > 30 && posX <90) || (posY == 16 && posX < 30) ||
               (posY == 24 && posX < 90) || posY == 35) return ("\u2500", true); //─
            return ("", false);
        }

        #endregion
    }
}
