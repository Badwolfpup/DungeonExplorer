using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMaster.Descriptions
{

    public static class RoomDescription
    {
        private static List<string> finalDesc = new List<string>();

        public static readonly List<string> RoomTypes = new List<string>
        {
            "A damp, dark corridor with the echo of dripping water.",
            "A narrow passageway shrouded in shadow and mystery.",
            "A cavernous hall of ancient stone, cold and foreboding.",
            "A modest chamber with time-worn walls and hidden secrets.",
            "A sprawling labyrinth of twisting corridors and secret nooks.",
            "A bleak, abandoned cell that whispers of lost souls.",
            "A grim, oppressive crypt lined with forgotten relics.",
            "A grand, vaulted hall with echoes of lost glory.",
            "A rustic, worn-out vault carved from raw rock.",
            "A cluttered antechamber, dimly lit by a solitary torch.",
            "A secluded, eerie vestibule that radiates decay.",
            "A small, claustrophobic passage with barely any light.",
            "A vaulted sanctuary long forsaken by time.",
            "A silent, empty corridor that feels frozen in history.",
            "A desolate chamber echoing with the weight of despair.",
            "A meandering passage with walls that seem to breathe.",
            "A secretive alcove hidden within the ancient fortress.",
            "A vast, echoing hall with a mysterious, haunting aura.",
            "A dilapidated room with crumbling stone and lingering mist.",
            "A somber, narrow corridor leading into the unknown."
        };

        // 2. Describes the walls, floor, and ceiling.
        public static readonly List<string> StructuralDetails = new List<string>
        {
            "The walls are slick with moisture, their surfaces worn smooth by time.",
            "Rugged stone walls enclose the space, rough and unyielding.",
            "The floor is uneven and scattered with ancient debris, while the ceiling arches high overhead.",
            "Cracked, moss-covered stones form the walls, with a cold, hard floor beneath.",
            "The walls bear deep carvings and the floor is strewn with scattered rubble.",
            "Weathered brick and mortar make up the walls, and the floor is blanketed in dust.",
            "The ceiling looms overhead, a vault of dark stone, with walls lined in faded murals.",
            "Lichen and creeping vines cover the stone surfaces, with a floor marked by time's decay.",
            "Rusted metal reinforcements mingle with crumbling stone on the walls, while the floor is littered with remnants of past battles.",
            "The walls, floor, and ceiling are etched with intricate designs, now obscured by the ravages of time.",
            "Jagged stone walls rise above a floor of shattered tiles, with a ceiling lost in shadow.",
            "The walls are adorned with peeling frescoes, and the floor is rough-hewn and cold.",
            "Polished marble walls reflect dim light off a cracked, worn floor.",
            "The rough, ancient stone walls are complemented by a dusty, uneven floor and a vaulted ceiling.",
            "The room's structure is defined by walls of weathered limestone, a floor littered with mortar, and a ceiling that fades into darkness.",
            "Crumbling plaster and exposed beams form the walls and ceiling, with a floor strewn with broken shards.",
            "Stalactites hang from a cavernous ceiling, with walls and floor carved by centuries of erosion.",
            "The walls bear the scars of past sieges, with a floor of cold, hard stone and a ceiling that echoes the passage of time.",
            "Smooth, worn stone walls, a cracked tiled floor, and a vaulted ceiling create an atmosphere of decay.",
            "The surfaces are rugged and raw: stone walls, a dust-covered floor, and a ceiling shrouded in darkness."
        };

        // 3. Describes the feeling or atmosphere.
        public static readonly List<string> Atmospheres = new List<string>
        {
            "An overwhelming sense of dread permeates the air.",
            "A feeling of solitude and melancholy fills your heart.",
            "A chill runs down your spine as you step inside.",
            "You feel a profound sadness mixed with quiet despair.",
            "An eerie calm settles over you, as if time itself has stopped.",
            "A lingering tension makes your every step cautious.",
            "A sense of foreboding grips you, warning of unseen dangers.",
            "You are engulfed by an inexplicable nostalgia for forgotten times.",
            "A quiet, oppressive gloom weighs on your soul.",
            "A surge of adrenaline and trepidation courses through your veins.",
            "You feel as though you're being watched by unseen eyes.",
            "A deep, unshakable loneliness pervades the space.",
            "An air of ancient sorrow surrounds you.",
            "A pulse of fear quickens your heartbeat as you enter.",
            "You experience an unsettling mixture of wonder and horror.",
            "A palpable aura of mystery and uncertainty envelops you.",
            "The atmosphere exudes both a haunting melancholy and a strange calm.",
            "You are overcome with an uncanny sense of timelessness.",
            "A chill of apprehension makes you wary of every shadow.",
            "An inexplicable tension leaves you breathless and alert."
        };

        // 4. Describes what you see in the room.
        public static readonly List<string> VisualDetails = new List<string>
        {
            "Flickering torches cast dancing shadows on the walls.",
            "Ancient tapestries hang in tatters from crumbling supports.",
            "Moss and lichen cling stubbornly to every surface.",
            "Scattered remnants of old armor and weapons lie in corners.",
            "Glittering crystals protrude from crevices, reflecting dim light.",
            "An ornate, broken chandelier dangles precariously from the ceiling.",
            "Faded murals hint at long-forgotten legends and battles.",
            "Rusted candelabras flicker weakly, adding to the gloom.",
            "Tattered banners and flags, remnants of past glories, flutter in the stale air.",
            "Shattered pottery and cracked statues are strewn across the floor.",
            "Dim shafts of light pierce through cracks in the ceiling.",
            "Ghostly silhouettes seem to move at the periphery of your vision.",
            "An overgrown vine creeps along a wall, softening its harsh lines.",
            "Ancient runes glow faintly on the stone, hinting at hidden magic.",
            "Dust motes dance in the narrow beams of light that seep in.",
            "A broken mirror reflects a distorted image of the room.",
            "Mysterious, faded symbols are etched into every surface.",
            "A pool of stagnant water mirrors the grim visage of the chamber.",
            "The remains of a once grand altar lie in ruin, draped in shadows.",
            "Gleaming relics and scattered trinkets hint at a storied past."
        };

        private static readonly Random rng = new Random();

        public static void GenerateRandomRoomDescription()
        {
            string type = RoomTypes[rng.Next(RoomTypes.Count)];
            string structure = StructuralDetails[rng.Next(StructuralDetails.Count)];
            string atmosphere = Atmospheres[rng.Next(Atmospheres.Count)];
            string visuals = VisualDetails[rng.Next(VisualDetails.Count)];
            List<string> desc = $"{type} {structure} {atmosphere} {visuals}".Split(" ").ToList();
            finalDesc = new List<string>();
            SplitText(desc);

        }

        private static void SplitText(List<string> desc)
        {
            string currentLine = "";
            int lineLength = 59;
            while (desc.Count > 0)
            {
                if (desc[0].Length + 1 < lineLength)
                {
                    currentLine += $"{(lineLength != 59 ? " " : "")}{desc[0]}";
                    lineLength -= desc[0].Length + 1;
                    desc.RemoveAt(0);
                }
                else
                {
                    finalDesc.Add(currentLine);
                    currentLine = "";
                    lineLength = 59;
                }

            }
            finalDesc.Add(currentLine);
        }

        public static void AddEventText(string text, bool lastline)
        {
            if (text == null) return;
            if (lastline) finalDesc.Add("");
            if (text.Length > 59) SplitText(text.Split(" ").ToList());
            else finalDesc.Add(text);
        }

        public static List<string> GetRandomRoomDescription() => finalDesc;

        public static void UpdateMonsterName(string name, string type)
        {
            if (name == null || type == null) return;

            switch (type)
            {
                case "Monster": finalDesc[finalDesc.Count - 1] = $"You see the corpse of the {name} lying on the floor"; break;
                case "Merchant": finalDesc[finalDesc.Count - 1] = $"The merchant is nowhere to be seen"; break;
            }
            
        }

        public static List<string> RoomLegend()
        {
            return new List<string> { 
                "A - Altar",
                "X - Battle",
                "B - Boss",
                "M - Merchant",
                "P - Prisoner",
                "? - Riddle",
                "^ - Stairs",
                "T - Teleport",
                "+ - Training",
                "* - Treasure", 
            };
        }

    }

}
