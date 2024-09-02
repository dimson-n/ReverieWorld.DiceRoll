## Reverie World dice roller

[![Build and Test](https://github.com/dimson-n/ReverieWorldDiceRoller/actions/workflows/test.yml/badge.svg)](https://github.com/dimson-n/ReverieWorldDiceRoller/actions/workflows/test.yml)

The dice roller implementation for Border of dreams RP system that provides main mechanics and extensibility by modifiers.

### Usage example

```csharp
using ReverieWorld.DiceRoll;

AutoRoller diceRoller = new(new DefaultRandomProvider());
Parameters parameters = new(dicesCount: 3, burstsCount: 1, bonus: 2);
Result result = diceRoller.Roll(parameters);

Console.WriteLine($"Roll result: {result.Total}");
```

### Support

You may support the author of the system (not me) on [Boosty](https://boosty.to/border_of_dreams).
