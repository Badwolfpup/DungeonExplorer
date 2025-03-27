using DungeonMaster.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMaster.Items
{
    public class RandomizedItem
    {
        public RandomizedItem()
        {
            TypeOfEffect = RandomizeTypeOfEffect();
            AddName();
            AddEffect();
            AddDescription();
            
        }

        public RandomizedItem(string type)
        {
            if (type == "Health")
            {
                TypeOfEffect = 51;
            }
            else if (type == "Mana")
            {
                TypeOfEffect = 26;
            }
            if (TypeOfEffect != 0)
            {
                AddName();
                AddEffect();
                AddDescription();
            }
        }

        public string Name { get; set; }
        private string Description { get; set; }
        private int TypeOfEffect;
        private Action Effect;
        private int RandomizeTypeOfEffect()
        {
            Random rnd = new Random();
            return rnd.Next(1,101);
        }

        public void Use()
        {
            Effect?.Invoke();
        }

        private void AddName()
        {
            Name = TypeOfEffect switch
            {
                > 95 => "",
                > 75 => "",
                > 50 => "Health potion",
                > 25 => "Mana potion",
                _ => "Grenade",
            };
        }

        private void AddEffect()
        {
            Effect = TypeOfEffect switch
            {
                > 95 => PermananetStatBoost,
                > 75 => TemporaryStatBoost,
                > 50 => AddHealthPoints,
                > 25 => AddManaPoints,
                _ => DamageToMonsters,
            };

        }

        private void AddDescription()
        {
            Description = TypeOfEffect switch
            {
                > 95 => "You hold up a small bottle with a neatly written label on it that says: Random statboost, Permanent effect",
                > 75 => "You hold up a dirty bottle. The label is smudged and hard to read. It takes a few seconds to decipher the text. It says: Random statboost, Permanent temporary",
                > 50 => "A red liquid swirls inside the bottle. It reminds you of blood. You have seen this type of liquid before and knows it will restore 25% of your health.",
                > 25 => "The bottle feels cold to your touch. If you weren't desperate to replenish your mana ou would throw it away. But 25% of your total mana back is worth any potential sideeffects.",
                _ => "It looks like a grenade. It ought to tickle the monsters at least",
            };
        }

        private void PermananetStatBoost()
        {
            Random rnd = new Random();
            int stat = rnd.Next(1, 4);
            switch (stat)
            {
                case 1:
                    int str = rnd.Next((int)((BaseIncreasePerClass(HolderClass.Instance.ChosenClass, "Strength")*(1+HolderClass.Instance.ChosenClass.Level/10.0))));
                    Console.WriteLine($"You feel yourself grow stronger. You have gained {str} strength");
                    Name = "Permanent Strength potion";
                    HolderClass.Instance.ChosenClass.BaseStrength += str;
                    break;
                case 2:
                    int dex = rnd.Next((int)((BaseIncreasePerClass(HolderClass.Instance.ChosenClass, "Dexterity") * (1 + HolderClass.Instance.ChosenClass.Level / 10.0))));
                    Console.WriteLine($"You feel yourself grow more nimble. You have gained {dex} dexterity");
                    Name = "Permanent Dexterity potion";
                    HolderClass.Instance.ChosenClass.BaseDexterity += dex;
                    break;
                case 3:
                    int intel = rnd.Next((int)((BaseIncreasePerClass(HolderClass.Instance.ChosenClass, "Intelligence") * (1 + HolderClass.Instance.ChosenClass.Level / 10.0))));
                    Console.WriteLine($"Suddenly the universe makes more sense. You have gained {intel} intelligence");
                    Name = "Permanent Intelligence potion";
                    HolderClass.Instance.ChosenClass.BaseIntelligence += intel;
                    break;
            }
        }

        private int BaseIncreasePerClass(BaseClass b, string stat)
        {
            if (HolderClass.Instance.ChosenClass.ClassName == "Warrior")
            {
                switch (stat)
                {
                    case "Strength":
                        return 10;
                    case "Dexterity":
                        return 5;
                    case "Intelligence":
                        return 2;
                }
            }
            else if (HolderClass.Instance.ChosenClass.ClassName == "Mage")
            {
                switch (stat)
                {
                    case "Strength":
                        return 2;
                    case "Dexterity":
                        return 5;
                    case "Intelligence":
                        return 10;
                }
            }
            else
            {
                switch (stat)
                {
                    case "Strength":
                        return 5;
                    case "Dexterity":
                        return 10;
                    case "Intelligence":
                        return 2;
                }
            }
            return 0;
        }

        private void DamageToMonsters()
        {
            Random rnd = new Random();
            int damage = (int)(HolderClass.Instance.ChosenClass.MaxHealth * (rnd.Next(10 - 26) / 100.0));
            HolderClass.Instance.ChosenClass.Health -= damage;
            Console.WriteLine($"You throw the grenade and it explodes in a shower of sparks and smoke. The monsters are confused and take {damage} damage");
            Console.WriteLine($"The {HolderClass.Instance.ChosenClass.Name} has {HolderClass.Instance.ChosenClass.Health} hp left");
        }

        private void AddManaPoints()
        {
            Console.WriteLine("You drink the potion and feel your mana replenish");
            HolderClass.Instance.ChosenClass.Mana += HolderClass.Instance.ChosenClass.Mana + (HolderClass.Instance.ChosenClass.MaxMana / 4) < HolderClass.Instance.ChosenClass.MaxMana ? HolderClass.Instance.ChosenClass.MaxMana / 4 : HolderClass.Instance.ChosenClass.MaxMana ;
        }

        private void AddHealthPoints()
        {
            Console.WriteLine("You drink the potion and feel your health replenish");
            HolderClass.Instance.ChosenClass.Health += HolderClass.Instance.ChosenClass.Health + (HolderClass.Instance.ChosenClass.MaxHealth / 4) < HolderClass.Instance.ChosenClass.MaxHealth ? HolderClass.Instance.ChosenClass.MaxHealth / 4 : HolderClass.Instance.ChosenClass.MaxHealth;
        }

        private void TemporaryStatBoost()
        {
            Random rnd = new Random();
            int stat = rnd.Next(1, 4);
            switch (stat)
            {
                case 1:
                    int str = rnd.Next((int)((BaseIncreasePerClass(HolderClass.Instance.ChosenClass, "Strength") * (1 + HolderClass.Instance.ChosenClass.Level / 10.0)))) * 2;
                    Console.WriteLine($"You feel yourself grow stronger. You have gained {str} strength. You can feel it's only a temporary boost though");
                    Name = "Temporary Strength potion";
                    HolderClass.Instance.ChosenClass.BaseStrength += str;
                    break;
                case 2:
                    int dex = rnd.Next((int)((BaseIncreasePerClass(HolderClass.Instance.ChosenClass, "Dexterity") * (1 + HolderClass.Instance.ChosenClass.Level / 10.0)))) * 2;
                    Console.WriteLine($"You feel yourself grow more nimble. You have gained {dex} dexterity. You can feel it's only a temporary boost though\"");
                    Name = "Temporary Dexterity potion";
                    HolderClass.Instance.ChosenClass.BaseDexterity += dex;
                    break;
                case 3:
                    int intel = rnd.Next((int)((BaseIncreasePerClass(HolderClass.Instance.ChosenClass, "Intelligence") * (1 + HolderClass.Instance.ChosenClass.Level / 10.0)))) * 2;
                    Console.WriteLine($"Suddenly the universe makes more sense. You have gained {intel} intelligence. You can feel it's only a temporary boost though\"");
                    Name = "Temporary Intelligence potion";
                    HolderClass.Instance.ChosenClass.BaseIntelligence += intel;
                    break;
            }
        }

        public string PrintDescription()
        {
            return Description;
        }

        public override string ToString()
        {
            return Name;
        }

    }
}
