using System;

namespace EvolutionSandbox
{
    internal class Agent : GameObject
    {
        double Energy = 100;
        float EnergyDecreaseRate = 1;
        public Agent(Vector2Int spawnPos, Guid id) : base(spawnPos, id, '*', GameObjectType.Agent)
        {
        }

        public override void Update(double deltaTime)
        {
            //Desrease energy
            Energy -= EnergyDecreaseRate * deltaTime;

            //Movement test
            MovementType randomMove = (MovementType)Program.GRND.Next(0, 12);
            MakeAction(new MoveAction(randomMove, GSPos, this));
        }

    }
}
