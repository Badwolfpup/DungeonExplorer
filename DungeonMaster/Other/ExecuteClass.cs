using DungeonMaster.Descriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMaster.Other
{
    public static class ExecuteClass
    {
        static public void Execute(Action a)
        {
            a?.Invoke();
            if (!HolderClass.Instance.SkipNextPrintOut) PrintUI.Print();
        }

        static public void ExecuteDirections(Action<string> a, string text)
        {
            a?.Invoke(text);
            if (!HolderClass.Instance.SkipNextPrintOut) PrintUI.Print();
        }
    }
}
