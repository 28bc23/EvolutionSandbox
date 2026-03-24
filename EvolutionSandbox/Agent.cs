using System;

namespace EvolutionSandbox
{
    internal class Agent : GameObject
    {
        static Random rnd;
        public Agent(Vector2Int spawnPos) : base(spawnPos)
        {
            rnd = new Random();
        }

        public override  void Update() //All actions in here will be made in one frame. Need to find way to work with it.
        {
            //Movement test
            MovementType randomMove = (MovementType)rnd.Next(0, 8);
            Grid.MoveAgent(Pos, randomMove);
        }

    }
}
