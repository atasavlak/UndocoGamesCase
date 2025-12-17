# Undoco Games -> Mini Game Case – Unity Project

## Overview
This project is a Unity-based mini game case developed to demonstrate clean gameplay flow, basic player mechanics, UI management, and a simple quiz system.

The primary focus of the project is gameplay clarity, technical structure, and scope control rather than visual complexity.

The project includes:
- A hub scene
- A submarine mini game
- A gameplay-triggered quiz system
- A result screen based on player performance

## Unity Version
Unity 2022.3.xf1 (LTS)

The project was developed and tested using Unity 2022.3 LTS to ensure stability.

## How to Run the Project
1. Clone the GitHub repository:
   git clone <repository-url>
2. Open Unity Hub
3. Click Open Project
4. Select the project folder
5. Make sure Unity 2022.3.xf1 is selected
6. Open the HubScene
7. Press Play
8. 
## Gameplay Flow
1. The player starts in the Hub Scene
2. From the hub, the player enters the Submarine Mini Game
3. In the Submarine Mini Game:
   - The player controls a submarine in a 2.5D environment
   - Treasure chests are placed in the level
   - The player must collect all chests
4. Once all chests are collected:
   - The score UI is hidden
   - The quiz panel is opened automatically
5. The player answers 5 quiz questions
6. After completing the quiz:
   - A result panel is displayed
   - The result message depends on the number of correct answers
7. From the result panel, the player can:
   - Restart the submarine mini game
   - Return to the hub scene

## Core Mechanics – Technical Explanation

### Submarine Movement
- Rigidbody-based movement system
- Smooth acceleration using Vector3.SmoothDamp
- Movement handled on the X and Y axes to create a 2.5D feel
- Direction-based rotation:
  - Y-axis rotation for left and right direction
  - Z-axis pitch for vertical movement

### Idle Bobbing System
- Idle bobbing is applied only to the visual child of the submarine
- Rigidbody and physics are not affected
- Bobbing is active only when the submarine is idle
- Bobbing stops immediately when movement starts
- Bobbing phase resets when movement begins to prevent jitter
- Provides a subtle underwater motion without affecting gameplay

### Knockback System
- Knockback is applied only when colliding with obstacles
- Treasure chests do not trigger knockback
- Player control is temporarily disabled during knockback
- Control is restored after a short duration

## Chest Collection System
- Treasure chests use trigger-based collision
- When a chest is collected:
  - It is removed from the scene
  - The chest counter UI is updated
- Collecting all chests triggers the quiz flow

## Quiz System
- Quiz questions are defined using ScriptableObjects
- The quiz consists of:
  - 5 questions
  - 3 options per question
- The system tracks:
  - Correct answers
  - Answered questions
- A live progress counter is displayed as:
  correct / answered

## Result Evaluation
After the quiz is completed, a result panel is shown with the following logic:
- 4–5 correct answers: Perfect!
- 2–3 correct answers: Well done!
- 0–1 correct answers: Good!

The result panel also displays the final score and provides navigation options.

## Architecture Notes
- SubmarineGameController acts as the central authority for gameplay flow
- QuizManager handles quiz logic only
- UI scripts are passive and contain no gameplay logic
- Scope is intentionally kept small to avoid over-engineering

## Gameplay Video
A short gameplay video demonstrating:

Video link: https://www.youtube.com/watch?v=5JzY0tcGWA8
