using DungeonMaster.Classes;
using DungeonMaster.Descriptions;
using DungeonMaster.Other;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMaster.Events
{
    public class Boss : IEvent
    {
        public Boss()
        {
            RoomDescription.GenerateRandomRoomDescription();
            RoomDescription.AddEventText(EventText(), true);
            Description = RoomDescription.GetRandomRoomDescription();
            HolderClass.Instance.IsBossFight = true;
        }


        private string EventText()
        {
            return $" A powerful aura emanates from {RandomizeBoss()}. He looks at you with a menacing look. He looks like he alot tougher than the other monsters.";
        }

        private string RandomizeBoss()
        {
            Random r = new Random();
            string name = MonsterNames.RandomBossName();
            //string monstername = MonsterNames.RandomMonsterFullName(name);
            HolderClass.Instance.Monster = new Monster(name, "", HolderClass.Instance.FloorLevel);
            var monster = HolderClass.Instance.Monster as Monster;
            if (monster != null) monster.BossStats();
            HolderClass.Instance.HasMonster = true;
            return name;
        }

        public string Type => "Boss";
        public List<string> Description { get; set; }

        public void BeforeNextRoom()
        {
            Labyrinth.SetRoomToSolved();
            HolderClass.Instance.SkipNextPrintOut = true;
            HolderClass.Instance.IsBossFight = false;
            if (HolderClass.Instance.ChosenClass.Health > 0) UpdateEventText();
            HolderClass.Instance.Save();

        }

        public void UpdateEventText()
        {
            int index = Description.IndexOf("");
            Description.RemoveRange(index + 1, Description.Count - index - 1);
            Description.Add($"You see the corpse of the {HolderClass.Instance.Monster.Name} lying on the floor");
        }

        public void Run()
        {
            Battle battle = new Battle(true);
            battle.Run();
        }

        public void SetDefaultOptions()
        {
        }

        public void SetUIState()
        {
        }
    }
}
