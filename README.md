# Slice_And_Dice_Unity_Game
A simple target shooting game where players click on moving targets to earn points, while avoiding bad targets that reduce their lives. The game features an explosion effect when a target is destroyed.

## Features

- **Game Difficulty**: Easy, Medium, Hard
- **Score System**: Earn points by hitting targets.
- **Lives**: Lose lives when a bad target is hit.
- **Explosion Effect**: Particle effects when targets are destroyed.
- **Pause/Restart**: Ability to pause and restart the game.

## GameManager

The `GameManager` class handles the game's logic, such as spawning targets, updating the score, managing lives, and controlling the overall game state (e.g., game over, pause).

### Key Methods:
- `StartGame(GameDifficulty difficulty)`: Starts the game based on the selected difficulty.
- `UpdateScore(int scoreToAdd)`: Updates the player's score.
- `UpdateLives(int livesToAdd)`: Updates the player's remaining lives.
- `DecLives()`: Decreases the player's lives by 1.
- `GameOver()`: Ends the game when the player runs out of lives.
- `OnRestartButtonClick()`: Restarts the game when the restart button is clicked.

## Target

The `Target` class represents the targets the player must click to score points. Some targets are bad and cause the player to lose lives.

### Key Methods:
- `DestroyTarget()`: Destroys the target, plays the explosion effect, and updates the score.
- `RandomForce()`: Generates a random upward force for the target.
- `RandomTorque()`: Generates random torque to make the target spin.
- `RandomSpawnPos()`: Generates a random spawn position for the target.

### Components:
- **TrailRenderer**: Adds a trail effect when targets are clicked.
- **BoxCollider**: Detects collisions with other objects.
- **ParticleSystem (Explosion)**: Plays an explosion effect when the target is destroyed.

## Game Flow

1. **Start Menu**: Select the difficulty (Easy, Medium, or Hard).
2. **Gameplay**: Targets appear at random positions and move upwards. Click on targets to destroy them and earn points.
3. **Game Over**: The game ends when the player runs out of lives. The player can restart the game.

## Controls

- **Mouse Click**: Click on the targets to destroy them.
- **P Key**: Pause the game and toggle the pause menu.
- **Restart Button**: Restart the game after game over.
