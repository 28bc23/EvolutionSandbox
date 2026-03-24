using System;

namespace EvolutionSandbox
{
    internal abstract class GameObject
    {
        public Vector2Int Pos { get; protected set; }
        public GameObject(Vector2Int spawnPos)
        {
            Pos = spawnPos;
            Grid.SpawnAgent(Pos);
        }
        public abstract void Update();

    }
}
