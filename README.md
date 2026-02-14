# DungeonExplorer

A console-based RPG dungeon crawler built in C# with a focus on object-oriented design. Explore procedurally generated labyrinths, fight monsters, collect equipment, solve riddles, and descend deeper into the dungeon.

## Technologies

- C#, .NET (Console Application)

## Features

- **3 playable classes** (Warrior, Mage, Ranger) with unique stats and abilities
- **Skill system** with melee specializations (Bash, Focus, Roar, Slam)
- **Equipment system** using interfaces (Head, Weapon, Chest) with randomized loot generation
- **12 event types** - Battles, Boss fights, Merchants, Prisoners, Riddles, Altars, Treasure rooms, Training rooms, Teleporters, Stairs, and more
- **Procedural labyrinth generation** with room descriptions
- **Monster system** with named enemies
- **Save/Load** system for persistent progress
- **Formatted console UI** with colored text output

## Architecture

```
DungeonMaster/
├── Classes/              # Character classes (BaseClass, Warrior, Mage, Ranger, Monster)
├── Skills/               # Skill system with melee specializations
├── Equipment/            # IEquipment interface, Head, Weapon, Chest, GenerateEquipment
├── Events/               # 12 event types implementing IEvent
├── Items/                # Randomized item generation
├── Descriptions/         # Room descriptions, monster/character names, UI printing
├── FormatClass/          # Console text color formatting
├── Other/                # Labyrinth, Room, SaveLoad, ExecuteClass, SetUIState
├── HolderClass.cs        # Game state container
└── Program.cs            # Entry point
```

## How to Run

Open `ConsoleAdventure.sln` in Visual Studio and run the DungeonMaster project.
