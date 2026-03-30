using System;

namespace EvolutionSandbox
{
    internal class Agent : GameObject
    {
        static Random rnd = new Random();
        public Agent(Vector2Int spawnPos, Guid id) : base(spawnPos, id, '*', GameObjectType.Agent)
        {
        }

        public override void Update()
        {
            //Movement test
            MovementType randomMove = (MovementType)rnd.Next(0, 12);
            MakeAction(new MoveAction(randomMove, GSPos, this));
        }

    }
}
