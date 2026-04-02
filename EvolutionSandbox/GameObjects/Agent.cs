using System;
using System.IO;

namespace EvolutionSandbox
{
    internal class Agent : GameObject
    {
        float EnergyDecreaseRate = 1;
        public Agent(Vector2Int spawnPos, Guid id) : base(spawnPos, id, '*', GameObjectType.Agent, 100)
        {
        }

        public override void Update()
        {
            //Desrease energy
            GSEnergy -= EnergyDecreaseRate * Program.GDeltaTime;
            if (GSEnergy <= 0)
            {
                Program.DestroyGameObject(this);
            }

            //Movement test
            MovementType randomMove = (MovementType)Program.GRND.Next(0, 12);
            MakeAction(new MoveAction(randomMove, GSPos, this));
        }

        public override void OnCollisionEnter(CollisionType collision, GameObject collidedGameObject)
        {
            if (collision != CollisionType.CollisionGameObject)
                return;

            if (collidedGameObject.GGameObjectType != GameObjectType.Food)
                return;

            GSEnergy += collidedGameObject.GSEnergy; // collidedGameObject should be food thanks to if statement above

            Program.DestroyGameObject(collidedGameObject);
        }

    }
}
