## Reverie World dice roller

[![Build and Test](https://github.com/dimson-n/ReverieWorldDiceRoller/actions/workflows/test.yml/badge.svg)](https://github.com/dimson-n/ReverieWorldDiceRoller/actions/workflows/test.yml)

The dice roller implementation for the Reverie World RP system that provides main mechanics and extensibility by modifiers.

### Usage example

```csharp
using ReverieWorld.DiceRoll;

AutoRoller diceRoller = new(new DefaultRandomProvider());
Parameters parameters = new(dicesCount: 3, burstsCount: 1, efficiency: 2);
SuccessParameters successParameters = new() { MinValue = 4 };
Result result = diceRoller.Roll(parameters, successParameters);

Console.WriteLine("Roll result: {0}", result.SuccessCount >= successParameters.Count ? "success!" : "failed...");
```
