# Project Name: SLIME RUSH

## Description:
This is a 2D platformer game project built in Unity, featuring a player character that can move, dash, and interact with coins and enemies.

## Features:

* Player movement and jumping
* Dashing ability with customizable speed and direction
* Coin collection system with score tracking
* Enemy interaction with damage and death mechanics
* Animation handling for player and enemies
* Collision detection and response for platforming and enemy interactions

## Scripts:

### PlayerScript.cs
The main script controlling the player character's movement, dashing, and interactions.

### Coin.cs
Script for coin objects, handling their value and collection.

### EnemyScript.cs
Script for enemy objects, handling their behavior and interactions with the player.

### GameManager.cs
Script managing game state, score, and player life.

## Components:

### Rigidbody2D
Used for player and enemy movement and collision detection.

### Collider2D
Used for collision detection and response.

### Animator
Used for player and enemy animations.

### SpriteRenderer
Used for rendering player and enemy sprites.

## Variables and Settings:

### movementSpeed
The speed at which the player moves.

### maxHorizontalVelocity and maxVerticalVelocity
The maximum speeds the player can reach on the horizontal and vertical axes.

### dashForce and dashTime
The force and duration of the player's dash ability.

### playerDamage
The amount of damage the player deals to enemies.

### maxHp and hp
The player's maximum and current health.

## Notes:

* The project uses Unity's 2D physics engine and animation system.
* The player's dash ability can be customized through the `dashForce` and `dashTime` variables.
* The coin collection system uses a static event system to notify other scripts of coin collection.
* The enemy interaction system uses a similar event system to notify other scripts of enemy damage and death.

## To-Do:

* Add more levels and platforming challenges
* Balance game difficulty and player abilities
* Add Lore
* Improve GameFeel
