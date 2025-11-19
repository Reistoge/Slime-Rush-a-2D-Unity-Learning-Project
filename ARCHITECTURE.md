# Slime Rush - Architecture Documentation

## Overview

Slime Rush is built using Unity 2022.3.15f1 with the Universal Render Pipeline (URP). The architecture follows Unity best practices while incorporating modern C# patterns and ScriptableObject-based design.

## Core Architectural Patterns

### 1. Singleton Pattern

Several managers use the Singleton pattern for centralized access:

- **GameManager**: Scene management, player lifecycle, runtime data
- **InputManager**: Input handling across all platforms (keyboard, touch, controller)
- **ShopSystem**: Shop state and item management

```csharp
public class InputManager : GenericSingleton<InputManager>
{
    // Accessible via InputManager.Instance
}
```

**Benefits**: 
- Single source of truth for global state
- Easy access from any script
- Prevents duplicate instances

**Trade-offs**: 
- Can lead to tight coupling if overused
- Difficult to test in isolation

### 2. Event System Architecture

The project uses two event systems for decoupled communication:

#### New EventSystem (Recommended)

Type-safe, centralized event management with compile-time checking:

```csharp
// Define event
public struct PlayerCoinCollected
{
    public int CoinValue;
}

// Subscribe
EventSystem.Subscribe<PlayerCoinCollected>(OnCoinCollected);

// Raise
EventSystem.Raise(new PlayerCoinCollected { CoinValue = 5 });

// Unsubscribe (in OnDisable)
EventSystem.Unsubscribe<PlayerCoinCollected>(OnCoinCollected);
```

**Event Definitions** (`GameEventDefinitions.cs`):
- Player events: Damage, Healing, Death, Dash, Coin collection
- Enemy events: Damage
- Game state events: Scene loading, Game restart
- Shop events: Purchase, Equipment

#### Legacy GameEvents (Being Phased Out)

Static action-based events for backward compatibility:

```csharp
// Subscribe
GameEvents.onMainGameSceneLoaded += HandleSceneLoad;

// Raise
GameEvents.triggerOnMainGameSceneLoaded();
```

**Migration Path**: New code should use EventSystem. Legacy code can be gradually migrated.

### 3. ScriptableObject Configuration

Configuration data is stored in ScriptableObjects for designer-friendly editing and runtime efficiency:

#### PlayerSO
Defines player characteristics:
- Movement settings (ground speed, air speed, max velocities)
- Combat stats (damage, health)
- Physics parameters (gravity, drag, mass)
- Dash configuration (force, time, charges)
- Jump settings (force, thresholds)

```csharp
[CreateAssetMenu(fileName = "PlayerVariant", menuName = "ScriptableObjects/Player")]
public class PlayerSO : ScriptableObject
{
    public float groundspeed;
    public int maxHp;
    public float dashForce;
    // ... etc
}
```

#### EnemyConfigSO
Configures enemy behavior:
- Basic stats (damage, health)
- Movement patterns (Stationary, Patrol, Chase, Circular, Flying)
- Detection types (Trigger, Collision, Raycast, Distance)
- Knockback settings
- Loot drops

#### CannonConfigSO
Defines cannon launch mechanics:
- Launch settings (speed, duration)
- Rotation behavior
- Player control during launch
- Auto-fire configuration
- Visual effects

#### GameSettingsSO
Global game configuration:
- Balance settings (starting coins, health)
- Physics parameters
- Camera settings
- Difficulty scaling
- Audio levels
- Debug options

**Benefits**:
- Non-programmers can adjust game balance
- Create variants without code changes
- Hot-reload changes in editor
- Asset-based versioning

### 4. Interface-Based Design

Interfaces define contracts for common behaviors:

#### IDamageable
For objects that can take damage:
```csharp
public interface IDamageable
{
    int Hp { get; set; }
    int MaxHp { get; set; }
    bool CanTakeDamage { get; set; }
    void takeDamage(int damage);
    void die();
}
```

Implemented by: PlayerScript, Enemy types

#### IEnemyBehaviour
For enemy attack patterns:
```csharp
public interface IEnemyBehaviour
{
    int Damage { get; set; }
    void dealDamage(GameObject target);
}
```

Implemented by: Spikes, StickySpikes, SpikesEnemy, etc.

#### Other Interfaces
- **IStickable**: Objects that can stick to surfaces
- **IBreakable**: Destructible platforms and objects
- **ILootable**: Objects that drop items/coins
- **IPurchasable**: Shop items

## System Communication Flow

### Player → Game Systems

```
Player Input
    ↓
InputManager (processes input)
    ↓
PlayerScript (executes action)
    ↓
EventSystem.Raise (notifies subscribers)
    ↓
UI/Audio/Effects (respond to event)
```

Example: Coin Collection
1. Player collides with coin (OnTriggerEnter2D)
2. Coin.getCoin() called
3. PlayerScript.playerGetCoin() invoked
4. EventSystem.Raise<PlayerCoinCollected>()
5. UI updates score
6. Audio plays coin sound

### Scene Management Flow

```
Portal/Cannon → GameManager.loadSceneWithTransition()
    ↓
Instantiate transition prefab
    ↓
Save runtime data
    ↓
SceneManager.LoadScene()
    ↓
OnLevelFinishedLoading()
    ↓
Spawn player
    ↓
EventSystem.Raise<SceneLoaded>()
```

### Shop System Flow

```
ShopSystem
    ├── HatsRepository (ScriptableObject data)
    ├── Selection UI (arrows, preview)
    └── Purchase/Equip buttons
         ↓
    GameManager.PlayerConfig
         ↓
    Player instantiation with equipment
```

## Key Components

### GameManager
**Responsibility**: Global game state

Key Functions:
- Scene loading with transitions
- Player lifecycle (spawn, death, restart)
- Runtime data persistence
- Camera effects (shake)

**Dependencies**:
- PlayerConfig (ScriptableObject)
- Scene assets
- Transition prefabs

### PlayerScript
**Responsibility**: Player physics and behavior

**Monolithic Warning**: This class is 1300+ lines and handles too many concerns. Future refactoring should split into:
- PlayerMovement
- PlayerHealth
- PlayerCombat
- PlayerAnimation

Current Structure:
- Movement (ground, air, rotation)
- Dash mechanics
- Jump system
- Collision handling
- Health management
- Cannon interactions

### InputManager
**Responsibility**: Unified input handling

Supports:
- Keyboard (WASD, arrows, space)
- Touch (swipe, tap)
- Virtual pad (mobile UI)

Features:
- Input acceleration curves
- Multi-platform detection
- Event-based notifications

### Cannon System
**Hierarchy**:
```
Cannon (base class)
├── DefaultCannon
├── FirstCannon
├── MainMenuCannon
├── ManualDirectionCannon
├── ManualRotateCannon
└── TargetShootCannon
```

Base Functionality:
- Player entry/exit
- Rotation animation
- Launch mechanics
- Sound/visual effects

Specialized behaviors extend base class.

## Data Flow

### Player Configuration

```
PlayerSO (ScriptableObject)
    ↓
PlayerScript.initializePlayerConfig()
    ↓
Apply to Rigidbody2D
    ↓
Runtime gameplay
    ↓
GameManager.PlayerRuntimeData
    ↓
Save across scenes
```

### Coin Collection

```
Coin → PlayerScript → EventSystem
                          ↓
                    ┌─────┼─────┐
                    ↓     ↓     ↓
                   UI   Audio  FX
```

### Damage System

```
Enemy/Hazard → IDamageable.takeDamage()
                    ↓
              Health -= damage
                    ↓
         EventSystem.Raise<PlayerDamaged>
                    ↓
              ┌─────┼─────┐
              ↓     ↓     ↓
            UI   Audio  Camera
          (hearts) (hurt) (shake)
              ↓
         if (hp <= 0)
              ↓
         EventSystem.Raise<PlayerDied>
              ↓
         GameOver UI
```

## Physics & Collision

### Layers
- Player
- Enemy
- Ground
- Collectibles
- Triggers

### Collision Detection
- **OnTriggerEnter2D**: Coins, triggers, damage zones
- **OnCollisionEnter2D**: Ground, walls, physical enemies

### Physics Materials
- Default: Standard friction
- Bounce: Used during dash
- Zero Friction: Used on certain surfaces

## Performance Considerations

### Current Optimizations
1. **Object Pooling**: Ghost trail effects use pooling
2. **Event Cleanup**: Unsubscribe in OnDisable to prevent leaks
3. **ScriptableObjects**: Data shared across instances
4. **Sprite Atlases**: Reduced draw calls
5. **Coroutine Management**: Stopped on state changes

### Future Optimizations
1. **Enemy Pooling**: Reuse enemy instances
2. **Particle Pooling**: Pool particle systems
3. **Raycasting**: Use layermasks, cache results
4. **Update Optimization**: Use FixedUpdate for physics only

## Testing Strategy

### Current State
- Manual testing in Unity Editor
- Build testing on target platforms

### Recommended Additions
1. **Unit Tests**: Test ScriptableObject configs
2. **Integration Tests**: Test event flows
3. **Play Mode Tests**: Test player mechanics
4. **Performance Tests**: Profile frame times

## Extension Points

### Adding New Enemy Types
1. Create `EnemyConfigSO` asset
2. Implement `IDamageable` and `IEnemyBehaviour`
3. Configure in Unity Inspector
4. Test damage and behavior

### Adding New Cannons
1. Create `CannonConfigSO` asset
2. Extend `Cannon` base class
3. Override `Start()` for custom behavior
4. Configure rotation/launch parameters

### Adding New Items
1. Implement `IPurchasable` or `ILootable`
2. Add to shop repository or loot tables
3. Configure visuals and pricing

## Migration Guide

### Moving from Legacy Events to EventSystem

**Before**:
```csharp
public static Action<int> OnPlayerGetCoin;

// Raise
OnPlayerGetCoin?.Invoke(coinValue);

// Subscribe
OnPlayerGetCoin += HandleCoinCollected;
```

**After**:
```csharp
// Raise
EventSystem.Raise(new PlayerCoinCollected { CoinValue = coinValue });

// Subscribe (OnEnable)
EventSystem.Subscribe<PlayerCoinCollected>(HandleCoinCollected);

// Unsubscribe (OnDisable)
EventSystem.Unsubscribe<PlayerCoinCollected>(HandleCoinCollected);

// Handler
void HandleCoinCollected(PlayerCoinCollected evt)
{
    // Use evt.CoinValue
}
```

## Troubleshooting

### Common Issues

**Problem**: Events not firing
- Check subscription in OnEnable, unsubscription in OnDisable
- Verify event is raised with correct type
- Check EventSystem.Clear() isn't being called prematurely

**Problem**: Player not spawning
- Verify "startPos" GameObject exists in scene
- Check GameManager.PlayerInScene is null
- Ensure player prefab is assigned

**Problem**: Cannon not launching
- Check canShoot flag
- Verify rotation complete (isRotating = false)
- Ensure player is inside barrel (inBarrel = true)

## Future Architecture Plans

1. **Component-Based Player**: Split PlayerScript into focused components
2. **Service Locator**: Alternative to singletons for testing
3. **Command Pattern**: Undo/redo for editor tools
4. **State Machine**: Formalize player states
5. **Dependency Injection**: For better testability
