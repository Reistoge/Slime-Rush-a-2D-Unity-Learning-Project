# Slime Rush - 2D Unity Platformer

![Unity Version](https://img.shields.io/badge/Unity-2022.3.15f1-blue)
![C#](https://img.shields.io/badge/C%23-Language-green)
![URP](https://img.shields.io/badge/URP-14.0.9-orange)

## Description

Slime Rush is a 2D platformer game built in Unity, featuring dynamic movement, dash mechanics, and engaging platforming challenges. The player navigates through levels filled with cannons, portals, collectibles, and various hazards.

## Features

### Core Gameplay
* **Advanced Movement System**: Ground and air movement with physics-based controls
* **Dash Mechanic**: Multi-directional dashing with limited charges that reset on landing
* **Jump System**: Charge-based jumping with directional control
* **Cannon Launching**: Travel through cannons that propel the player at different angles
* **Portal System**: Scene transitions with smooth animations

### Game Systems
* **Coin Collection**: Collectible coins with score tracking and persistence
* **Health System**: Player health with damage, healing, and heart upgrades
* **Shop System**: Purchase and equip cosmetic items (hats)
* **Enemy Interactions**: Various enemy types with different behaviors (spikes, sticky traps, etc.)
* **Breakable Platforms**: Time-based and trigger-based destructible platforms
* **Visual Effects**: Ghost trails, particle effects, and camera shake

## Project Structure

```
Assets/
├── Scripts/
│   ├── Core/                    # Core systems (EventSystem, GameEventDefinitions)
│   ├── Player/                  # Player scripts (PlayerScript, PlayerAnimationHandler, PlayerAudioSystem)
│   │   └── SO/                  # Player ScriptableObject configurations
│   ├── Enemy/                   # Enemy types (Spikes, StickySpikes, etc.)
│   ├── Cannon/                  # Cannon system for launching player
│   ├── Coins/                   # Coin collection system
│   ├── UI/                      # UI components (menus, HUD, game over)
│   ├── Utils/                   # Utility scripts and interfaces
│   ├── InputManager/            # Input handling (touch, keyboard, controller)
│   ├── Camera/                  # Camera follow and effects
│   ├── Portal/                  # Portal and scene transition system
│   ├── Wall/                    # Wall types (bounce walls, breakable floors)
│   ├── items/                   # Item system (heal items, item boxes)
│   └── DangerZone/              # Level management and platform spawning
├── ShopSystem/                  # Shop functionality for purchasing items
├── Prefabs/                     # Game object prefabs
├── Scenes/                      # Game scenes
└── Animation/                   # Animation controllers and clips
```

## Architecture

### Event System
The project uses a centralized `EventSystem` for decoupled communication between systems:
- **Typed Events**: Strongly-typed event definitions in `GameEventDefinitions.cs`
- **Subscribe/Unsubscribe**: Components subscribe to events in `OnEnable`/`OnDisable`
- **Event Publishing**: Systems raise events without needing direct references

Example:
```csharp
// Subscribe to player damage event
EventSystem.Subscribe<PlayerDamaged>(OnPlayerTakeDamage);

// Raise an event
EventSystem.Raise(new PlayerDamaged { Damage = 1, RemainingHealth = 2 });
```

### ScriptableObject Configuration
Player configuration is managed through `PlayerSO`:
- Movement settings (ground speed, air speed, max velocities)
- Combat stats (damage, health)
- Physics parameters (gravity, drag, mass)
- Dash settings (force, time, max counter)
- Jump settings (force, thresholds)

### Singleton Managers
- **GameManager**: Scene management, player lifecycle, runtime data persistence
- **InputManager**: Centralized input handling for all control schemes
- **ShopSystem**: Shop state and item management

## Unity Version & Dependencies

- **Unity Version**: 2022.3.15f1 (LTS)
- **Render Pipeline**: Universal Render Pipeline (URP) 14.0.9
- **Input System**: Unity Input System 1.7.0
- **TextMeshPro**: 3.0.6
- **Timeline**: 1.7.6

## Setup Instructions

### Prerequisites
1. Install Unity 2022.3.15f1 or later (LTS recommended)
2. Install Unity modules:
   - Universal Windows Platform Build Support (if building for Windows)
   - Android Build Support (if building for mobile)

### Installation
1. Clone the repository:
   ```bash
   git clone https://github.com/Reistoge/Slime-Rush-a-2D-Unity-Learning-Project.git
   ```

2. Open the project in Unity Hub:
   - Click "Open" in Unity Hub
   - Navigate to the cloned folder and select it
   - Wait for Unity to import all assets

3. Open the main scene:
   - Navigate to `Assets/Scenes/Menu.unity`
   - Press Play to start the game

### Build Instructions

#### Windows
1. File → Build Settings
2. Select "Windows, Mac, Linux"
3. Click "Build" and choose output folder

#### Android
1. File → Build Settings
2. Select "Android"
3. Click "Player Settings"
4. Configure package name and version
5. Click "Build" or "Build and Run"

## Controls

### Keyboard (PC)
- **Arrow Keys / WASD**: Move left/right
- **Space (Hold & Release)**: Charge and jump
- **Space (in air)**: Dash

### Touch (Mobile)
- **Swipe Up**: Jump
- **Swipe (in air)**: Dash in swipe direction
- **Touch Left/Right**: Move left/right

## Code Style Guidelines

This project follows the coding standards defined in `.editorconfig`:
- **Naming**: PascalCase for public members, camelCase with underscore prefix for private fields
- **Indentation**: 4 spaces for C# files
- **Documentation**: XML documentation comments for all public APIs
- **Braces**: Always use braces for control statements

## Best Practices & Coding Standards

### Code Organization
- **Use Regions**: Organize code into logical regions (#region/#endregion) for better navigation
  - `#region Serialized Fields` - Unity Inspector fields
  - `#region Private Fields` - Private class members
  - `#region Properties` - Public properties
  - `#region Unity Lifecycle` - Unity methods (Start, Update, OnEnable, etc.)
  - `#region Public Methods` - Public API
  - `#region Private Methods` - Internal helper methods
  - `#region Coroutines` - Coroutine methods
  - `#region Enums` - Enum definitions

### Documentation Standards
- **Always add XML documentation** for public classes, methods, and properties
- Include `<summary>` tags explaining what the code does
- Add `<param>` tags for method parameters
- Include `<returns>` tags for non-void methods
- Use `<remarks>` for additional context or warnings
- Example:
  ```csharp
  /// <summary>
  /// Spawns a platform at the specified position.
  /// </summary>
  /// <param name="position">The world position to spawn the platform</param>
  /// <returns>The spawned platform GameObject</returns>
  public GameObject SpawnPlatform(Vector2 position)
  {
      return Instantiate(platformPrefab, position, Quaternion.identity);
  }
  ```

### Event System Usage
- **Prefer the new EventSystem** over legacy static events for new code
- Always unsubscribe in `OnDisable()` to prevent memory leaks
- Use strongly-typed event structs defined in `GameEventDefinitions.cs`
- Example:
  ```csharp
  void OnEnable()
  {
      EventSystem.Subscribe<PlayerDamaged>(OnPlayerTakeDamage);
  }
  
  void OnDisable()
  {
      EventSystem.Unsubscribe<PlayerDamaged>(OnPlayerTakeDamage);
  }
  
  void OnPlayerTakeDamage(PlayerDamaged evt)
  {
      Debug.Log($"Player took {evt.Damage} damage");
  }
  ```

### ScriptableObject Configuration
- **Use ScriptableObjects for game data** instead of hard-coded values
- Create variants for different gameplay scenarios
- Add tooltips to all serialized fields for designer clarity
- Example:
  ```csharp
  [CreateAssetMenu(fileName = "NewEnemy", menuName = "Game/Enemy Config")]
  public class EnemyConfigSO : ScriptableObject
  {
      [Tooltip("Enemy maximum health points")]
      public int maxHealth = 100;
      
      [Tooltip("Damage dealt per hit")]
      public int damage = 10;
  }
  ```

### Common Pitfalls to Avoid

#### 1. Memory Leaks from Events
❌ **Bad**: Not unsubscribing from events
```csharp
void Start()
{
    EventSystem.Subscribe<PlayerDied>(OnPlayerDied);
    // Missing unsubscribe in OnDisable!
}
```

✅ **Good**: Always pair Subscribe with Unsubscribe
```csharp
void OnEnable()
{
    EventSystem.Subscribe<PlayerDied>(OnPlayerDied);
}

void OnDisable()
{
    EventSystem.Unsubscribe<PlayerDied>(OnPlayerDied);
}
```

#### 2. Finding Objects Every Frame
❌ **Bad**: Using Find in Update
```csharp
void Update()
{
    GameObject player = GameObject.Find("Player");
    // Expensive operation every frame!
}
```

✅ **Good**: Cache references
```csharp
private GameObject player;

void Start()
{
    player = GameObject.Find("Player");
}

void Update()
{
    // Use cached reference
}
```

#### 3. Inconsistent Naming
❌ **Bad**: Mixed naming conventions
```csharp
public void shoot_cannon() { }  // snake_case
public void JumpPlayer() { }     // PascalCase
private int MyHealth;            // PascalCase for private
```

✅ **Good**: Follow C# conventions
```csharp
public void ShootCannon() { }    // PascalCase for public methods
public void JumpPlayer() { }     // PascalCase for public methods
private int _myHealth;           // camelCase with _ prefix for private
```

#### 4. Not Using Tooltips
❌ **Bad**: No explanation for designers
```csharp
[SerializeField] private float jumpForce;
[SerializeField] private int damage;
```

✅ **Good**: Add tooltips for clarity
```csharp
[Tooltip("Force applied when player jumps (higher = higher jump)")]
[SerializeField] private float jumpForce = 10f;

[Tooltip("Damage dealt to enemies on contact")]
[SerializeField] private int damage = 25;
```

#### 5. Unused Imports
❌ **Bad**: Including unnecessary namespaces
```csharp
using System.Data;
using System.Numerics;
using UnityEngine.Video;
// Most of these are unused!
```

✅ **Good**: Only include what you need
```csharp
using UnityEngine;
using System.Collections;
```

### Refactoring Guidelines

When refactoring code:
1. **Make small, incremental changes** - Don't try to refactor everything at once
2. **Test after each change** - Ensure functionality is preserved
3. **Document as you go** - Add XML comments while the code is fresh in your mind
4. **Use regions to organize** - Makes large files more manageable
5. **Extract magic numbers** - Replace hard-coded values with named constants
6. **Consider ScriptableObjects** - For values that should be configurable
7. **Break up god classes** - Split classes with > 500 lines into smaller components

### Performance Best Practices

- **Use object pooling** for frequently spawned/destroyed objects (bullets, particles, enemies)
- **Cache component references** instead of using GetComponent repeatedly
- **Avoid allocations in Update** - No `new` operations in hot paths
- **Use layermasks with raycasts** - Reduces unnecessary collision checks
- **Disable inactive GameObjects** rather than destroying them if they'll be reused

### Recommended Patterns

#### Singleton Pattern (for Managers)
```csharp
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
```

#### Command Pattern (for Undoable Actions)
```csharp
public interface ICommand
{
    void Execute();
    void Undo();
}

public class MoveCommand : ICommand
{
    private Transform transform;
    private Vector3 movement;
    
    public void Execute()
    {
        transform.position += movement;
    }
    
    public void Undo()
    {
        transform.position -= movement;
    }
}
```

#### Object Pool Pattern
```csharp
public class ObjectPool<T> where T : Component
{
    private Queue<T> pool = new Queue<T>();
    private T prefab;
    
    public T Get()
    {
        if (pool.Count > 0)
        {
            T obj = pool.Dequeue();
            obj.gameObject.SetActive(true);
            return obj;
        }
        return Object.Instantiate(prefab);
    }
    
    public void Return(T obj)
    {
        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
    }
}
```

## Recent Improvements

### Refactoring Updates
- ✅ Reorganized folder structure for better maintainability
- ✅ Added centralized EventSystem for decoupled communication
- ✅ Comprehensive XML documentation for public APIs
- ✅ Standardized code formatting with `.editorconfig`
- ✅ Enhanced interfaces with detailed documentation
- ✅ Improved code readability with regions and comments
- ✅ Removed unused imports and cleaned up code

## Contributing

When contributing to this project:
1. Follow the code style guidelines in `.editorconfig`
2. Add XML documentation for all public classes and methods
3. Use the EventSystem for inter-system communication
4. Test changes thoroughly before submitting
5. Keep pull requests focused on a single feature or fix
6. Follow the best practices outlined above
7. Use regions to organize your code
8. Add tooltips to all serialized fields

## Future Enhancements

* Refactor large monolithic classes (PlayerScript, GameManager) into smaller components
* Expand ScriptableObject usage for enemy and level configurations
* Implement pooling for frequently spawned objects
* Add more levels and platforming challenges
* Balance game difficulty and player abilities
* Develop narrative and lore
* Enhance game feel with improved juice and polish
* Add unit tests for core gameplay systems
* Implement save/load system for player progress
