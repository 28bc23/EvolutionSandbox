# EvolutionSandbox

> [!Warning]
> Checkpoint readme is on commit [8aa675b](https://github.com/28bc23/EvolutionSandbox/tree/8aa675b31020e78a21333210794c0a2313a27425)

A console application in C# (.NET 10) simulating the evolution of agents in a 2D grid. Agents survive by finding and consuming food, which replenishes their energy. If an agent's energy drops to zero, it dies. The goal is to evolve agents capable of efficient survival using evolutionary algorithms and neural networks.

## Main functions
- Neural network dynamically expanding/changing using mutations
- Custom evolution algorithm
- Customization of environment for training
- Easy code modification - adding own game objects, actions, ...
- PCG-XSH-RR generator for better deterministic training
- Graph generation

## Evolutionary algorithm
### Start
It creates the first generation of agents with a randomly initialized neural network and applied mutations
### End and Creation of new generation
A generation ends either when all agents die or when the time limit expires. The agents are then ranked by score, from worst to best. A new generation is then formed from the upper half (the better agents), and a mutation of neural network is applied to it.
### Score function
$Score = (FoodScoreCoef * FoodEaten) + (EnergyBonusCoef * energyBonus) + (CloseToFoodBonusCoef * closeToFoodBonus) + (CenterBonusCoef * centerBonus)$

$energyBonus = \frac{Energy - MaxEnergy}{MaxEnergy}$

$closeToFoodBonus = \frac{\sqrt{GridSize.X^2 + GridSize.Y^2} - \sqrt{(Pos.X - closestFoodPos.X)^2 + (Pos.Y - closestFoodPos.Y)^2}}{\sqrt{GridSize.X^2 + GridSize.Y^2}}$

## Installation and execution
1. Download .NET 10
2. use dotnet run or Visual Studio to compile and run sandbox

## Configuration
**The same configuration should always result in the same training.**
|Attribute|Description|Example|
|-----------|-------------|---------|
|EnvName|Used to determine file names and their locations. (string)|TestEnv|
|GridSizeX|Sets the grid size along the X-axis. **Only positive 32-bit numbers can be used (uint)**|30|
|GridSizeY|Sets the grid size along the Y-axis. **Only positive 32-bit numbers can be used (uint)**|30|
|Seed|The seed value for generating random numbers. **Only positive 64-bit numbers can be used (ulong)**|15240|
|FpsCap|sets the maximum number of frames per second. (int)|60|
|TPS|Sets the number of ticks per second. Determines how many times the game logic runs per second. **Only positive 32-bit numbers can be used (uint)**|1|
|TimeScale|Sets the game speed. **High values can cause lags.** (float)|10.0|
|MaxTicksPerFrame|Sets the maximum number of ticks per frame. **Recommended value: 3.** (double)|3.0|
|NumAgents|Specifies how many agents should be spawned when creating a new generation. (int)|100|
|GenerationTime|Sets the maximum duration of a single generation. (float)|150.0|
|MaxFoodInEnv|Sets the maximum number of foods in the environment. (int)|150|
|FoodSpawnRate|Sets the amount of food that spawns per second. (float)|50.0|
|FoodEnergy|Determines how much energy the food provides to the agent when eaten. (float)|300.0|
|AgentMaxEnergy|Sets the amount of energy the agent has upon spawning. (float)|500.0|
|AgentEnergyDecreaseRate|Sets the amount by which the agent's energy decreases per second. (float)|1.0|
|AgentWallCollisionEnergyPenalty|Determines how much energy the agent will lose if it collides with a wall. (float)|10.0|
|AgentStepActionEnergyCost|Determines how much energy it costs an agent to take one step (movement of 1 space). (float)|1.0|
|AgentJumpActionEnergyCost|Determines how much energy it costs an agent to jump (movement of 2 spaces). (float)|3.0|
|AgentNoActionEnergyCost|Determines how much energy it costs an agent to remain stationary. (float)|0.0|
|GraphRate|Specifies how often the graph will be saved. (A value of 2 will save the graph every 2 generations). (int)|100|
|FoodScoreCoef|Sets the weight of the amount of food consumed when calculating the agent's score. (float)|2.0|
|EnergyBonusCoef|Sets the weight of the energy bonus when calculating an agent's score. (float)|1.0|
|CloseToFoodBonusCoef|Sets the weight of the bonus for closeness to food when calculating the agent's score. (float)|1.5|
|CenterBonusCoef|Sets the weight for the center bonus when calculating the agent's score. (float)|0.0|
|WeightMutationChance|Sets the probability of a weight mutation occurring. Value range: 0–1. (float)|0.4|
|BiasMutationChance|Sets the probability of a bias mutation occurring. Value range: 0–1. (float)|0.2|
|SplitMutationChance|Sets the probability of a split mutation occurring. Value range: 0–1. (float)|0.05|
|NewConnectionMutationChance|Sets the probability of a new connection forming between nodes. Value range: 0–1. (float)|0.1|
|NewNodeMutationChance|Sets the probability of new nodes being created. Value range: 0–1. (float)|0.05|
|WeightMutationSizeMin|Sets the lower limit for random weight mutation. (float)|-1.0|
|WeightMutationSizeMax|Sets the upper limit for random weight mutation. (float)|1.0|
|BiasMutationSizeMin|Sets the lower limit for random weight mutation. (float)|-0.5|
|BiasMutationSizeMax|Sets the upper limit for random bias mutation. (float)|0.5|

## Commands
All commands must start with a colon `:`.

**Risk Levels:**
* `✓` : Safe to use anytime.
* `!!` : Impacts evolution balance or performance.
* `!!!` : High risk (requires restart or breaks logic).

| Command | Description | Risk |
|:---|:---|:---:|
| `:graph` | Save current training graph | ✓ |
| `:save` | Create manual checkpoint | ✓ |
| `:save-config` | Save current settings to file | ✓ |
| `:quit` | Save and exit | ✓ |
| `:quit!` | Exit without saving | ✓ |
| `:fps-cap [val]` | Frame rate limit | ✓ |
| `:graph-rate [val]` | Auto-graph frequency | ✓ |
| `:time-scale [val]` | Simulation speed multiplier | !! |
| `:max-ticks-per-frame [val]` | Max logic steps per frame | !! |
| `:num-agents [val]` | Agent count for next generation | !! |
| `:gen-time [val]` | Generation duration | !! |
| `:max-food-in-env [val]` | Max food count | !! |
| `:food-spawn-rate [val]` | Food spawn speed | !! |
| `:food-energy [val]` | Energy per food | !! |
| `:agent-max-energy [val]` | Starting/Max energy | !! |
| `:agent-energy-drate [val]` | Energy decay rate | !! |
| `:agent-wall-penalty [val]` | Wall collision penalty | !! |
| `:agent-step-cost [val]` | Cost to move 1 tile | !! |
| `:agent-jump-cost [val]` | Cost to jump 2 tiles | !! |
| `:agent-noact-cost [val]` | Cost to stay idle | !! |
| `:food-score-coef [val]` | Food fitness weight | !! |
| `:energy-bonus-coef [val]` | Energy fitness weight | !! |
| `:ctf-bonus-coef [val]` | Proximity to food fitness weight | !! |
| `:center-bonus-coef [val]` | Center proximity fitness weight | !! |
| `:weight-mutation-chance [val]` | Weight mutation probability | !! |
| `:bias-mutation-chance [val]` | Bias mutation probability | !! |
| `:split-mutation-chance [val]` | Link split probability | !! |
| `:new-connection-mutation-chance [val]` | New link probability | !! |
| `:new-node-mutation-chance [val]` | New node probability | !! |
| `:weight-mutation-size-min [val]` | Weight mutation range lower| !! |
| `:weight-mutation-size-max [val]` | Weight mutation range higher| !! |
| `:bias-mutation-size-min [val]` | Bias mutation range lower| !! |
| `:bias-mutation-size-max [val]` | Bias mutation range higher| !! |
| `:grid-size-x [val]` | Grid width (requires restart) | !!! |
| `:grid-size-y [val]` | Grid height (requires restart) | !!! |
| `:seed [val]` | Change RNG seed (breaks determinism) | !!! |
| `:tps [val]` | Logic updates per second | !!! |

## TODO
- loading checkpoint
## Future expansion
- Aggressive interactions between agents.
- A more diverse environment
- Crossbreeding of Agents

## Time required
The project was developed on an ongoing basis over a period of 7 weeks (equivalent to approximately 2 months). I estimate that a total of 40–60 hours was spent on the project.
