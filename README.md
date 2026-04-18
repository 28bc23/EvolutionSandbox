# EvolutionSandbox - Project Checkpoint

> [!Warning]
> This checkpoint is for commit `cc17032` - *merge main to NN - LICENCE and checkpoint readme* on main branch

A console application in C# (.NET Framework) simulating the evolution of agents in a 2D grid. Agents survive by finding and consuming food, which replenishes their energy. If an agent's energy drops to zero, it dies. The goal is to evolve agents capable of efficient survival using evolutionary algorithms and neural networks.

## 1. Completed Work
**Infrastructure and Core:**
- **Game Loop:** Implemented in `Program.cs`. Includes `DeltaTime` calculation, independent logic timing (Actions Per Second - APS), and rendering limiter (FPS cap).
- **Grid Management (`Grid.cs`):** 2D array for storing entities. Handles collision detection, object movement, and rendering the game state to the console.
- **Entity System (`GameObject.cs`):** Abstract class defining basic object properties (position, energy, ID, action queue, collision detection).

**Game Mechanics:**
- **Action System (`ActionSystem.cs`):** Agents interact with the environment using an action queue. `MoveAction` is implemented with various movement types (orthogonal, diagonal, jumps) and defined energy costs.
- **Agents (`Agent.cs`):** Entity with energy that gradually decreases over time (`DeltaTime`). The agent is removed when energy reaches zero. It can consume food upon collision. Movement is currently randomized (for grid testing purposes).
- **Food and Management (`Food.cs`, `FoodManager.cs`):** Classes ensuring the existence of food (adds energy) and its automatic replenishment at random positions up to a specified limit.
- **Neural Network (NN):** Forward pass is drafted, currently in an untested prototype phase waiting for integration. (On NN branch)

## 2. TODO

- **Full Neural Network Integration:** Connecting the NN to the agent's decision-making process (replacing the current random movement). The agent must generate output actions (movement) based on inputs (food direction/position).
- **Evolution Cycle and Evaluation:** Implementation of a mechanism that evaluates the success of individuals (e.g., survival time, amount of food eaten) after a time limit expires (or all agents die).
- **Genetic Algorithms:**
  - Creation of new generations.
  - Mutation of neural network.
  - Crossbreeding of the most successful agents.
- **Sensor Expansion:** Agents need the ability to "see" their surroundings or the direction to the nearest food for the neural network to function.

## 3. Potential Expansions
- Aggressive interactions (agents can attack each other).
- Increased environment variability (multiple food types, different agent types with distinct properties).
- More complex obstacles within the grid (not just border walls).
- Implementation of a combination of evolutionary learning and backpropagation
