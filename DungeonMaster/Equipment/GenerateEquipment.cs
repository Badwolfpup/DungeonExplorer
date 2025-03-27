using DungeonMaster.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMaster.Equipment
{
    public static class GenerateEquipment
    {
        private static List<string> _qualityofequipment = new List<string> { "Copper", "Bronze", "Iron", "Steel", "Cobolt", "Diamond", "Platinum" };

        public static IEquipment Chest(string chosenclass, int factor)
        {
            Random rnd = new Random();
            switch (chosenclass)
            {
                case "Warrior": return new Chest(GenerateName("Chest"), 8, 5, 3, factor);
                case "Mage": return new Chest(GenerateName("Chest"), 3, 5, 8, factor);
                case "Ranger": return new Chest(GenerateName("Chest"), 5, 8, 3, factor);
                default: return new Chest("Default chest");
            }
        }

        public static IEquipment Head(string chosenclass, int factor)
        {
            Random rnd = new Random();
            switch (chosenclass)
            {
                case "Warrior": return new Head(GenerateName("Head"), 8, 5, 3, factor);
                case "Mage": return new Head(GenerateName("Head"), 3, 5, 8, factor);
                case "Ranger": return new Head(GenerateName("Head"), 5, 8, 3, factor);
                default: return new Head("Default head");
            }
        }

        public static IEquipment Weapon(string chosenclass, int factor)
        {
            Random rnd = new Random();
            switch (chosenclass)
            {
                case "Warrior": return new Weapon(GenerateName("Weapon"), 8, 5, 3, factor);
                case "Mage": return new Weapon(GenerateName("Weapon"), 3, 5, 8, factor);
                case "Ranger": return new Weapon(GenerateName("Weapon"), 5, 8, 3, factor);
                default: return new Weapon("Default weapon");
            }
        }

        private static string GenerateName(string armortype)
        {
            Random rnd = new Random();
            return _qualityofequipment[rnd.Next(0, _qualityofequipment.Count)] + " " + armortype;
        }

        public static IEquipment RandomEquipment()
        {
            Random rnd = new Random();

            var ChosenClass = HolderClass.Instance.ChosenClass;
            IEquipment looteditem;
            switch (rnd.Next(1, 4))
            {
                case 1:
                    looteditem = Chest(ChosenClass.ClassName, ChosenClass.Level);
                    break;
                case 2:
                    looteditem = Head(ChosenClass.ClassName, ChosenClass.Level);
                    break;
                case 3:
                    looteditem = Weapon(ChosenClass.ClassName, ChosenClass.Level);
                    break;
                default:
                    looteditem = new Weapon("Wooden sword");
                    break;
            }
            return looteditem;
        }
    }
}
