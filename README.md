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
â”œâ”€â”€ Classes/              # Character classes (BaseClass, Warrior, Mage, Ranger, Monster)
â”œâ”€â”€ Skills/               # Skill system with melee specializations
â”œâ”€â”€ Equipment/            # IEquipment interface, Head, Weapon, Chest, GenerateEquipment
â”œâ”€â”€ Events/               # 12 event types implementing IEvent
â”œâ”€â”€ Items/                # Randomized item generation
â”œâ”€â”€ Descriptions/         # Room descriptions, monster/character names, UI printing
â”œâ”€â”€ FormatClass/          # Console text color formatting
â”œâ”€â”€ Other/                # Labyrinth, Room, SaveLoad, ExecuteClass, SetUIState
â”œâ”€â”€ HolderClass.cs        # Game state container
â””â”€â”€ Program.cs            # Entry point
```

## How to Run

Open `ConsoleAdventure.sln` in Visual Studio and run the DungeonMaster project.
