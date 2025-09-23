# FusionFrame

## Overview
**Fusion Frame** is a puzzle-action game made in Unity for a game jam.  
The twist: you don’t control one blob—you control *all* blobs at the same time with a single set of inputs.  

The game starts with multiple split-screen views (Level 1 = 2 screens, Level 2 = 3 screens, etc.).  
Each screen contains one blob. The goal of each level is to **merge all blobs into a single group**.

---

## Core Mechanics
- **Shared Controls**  
  All blobs respond to the same input (WASD or Arrow Keys).  

- **Split Screens**  
  Each blob gets its own camera tile. When blobs merge, their screens combine into one.  

- **Sticky Internal Borders**  
  When a blob touches the **divider** between screens, it sticks to the wall:
  - Movement is constrained along the divider.
  - Speed is reduced slightly.  
  This makes it easier to align and merge blobs moving in the same direction.

- **Walls**  
  Placed in the playfield, they block movement.  
  Slanted walls allow blobs to **slide along the surface tangent** for rail-like movement.  

- **Hazards**  
  Touching hazards reduces a blob’s health.  
  If all blobs die, the level restarts.  

- **Level Progression**  
  - Level 1: 2 blobs/screens.  
  - Each cleared level adds +1 blob/screen.  
  - Merge all blobs into one to advance.

---

## How to Play
1. **Move blobs** using **WASD** or **Arrow Keys**.  
   > Both control schemes work; all blobs move simultaneously.  

2. **Stick to dividers**: Push blobs against internal split borders to slide them into alignment.  

3. **Merge blobs**: Get blobs from adjacent screens to touch each other—screens merge too.  

4. **Avoid hazards**: Touching hazards decreases health. If all blobs die, you must restart the level.  

5. **Clear the level** by merging all blobs into a single group. Progress to the next level with more blobs/screens.  

---

## Controls
- **WASD / Arrow Keys** — Move blobs  
- **Esc / Alt+F4** — Quit (standard OS behavior)

---

## Tips
- Use sticky borders to “line up” blobs before merging.  
- Slanted walls can help funnel blobs into each other—but they can also lead into hazards.  
- Plan merges carefully in later levels where more screens and obstacles appear.  

---