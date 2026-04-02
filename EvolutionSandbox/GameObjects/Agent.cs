using System;
using System.IO;

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
            Console.WriteLine(Energy);
            if (Energy <= 0)
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

            Food food = (Food)collidedGameObject; //  Should always parse
            Energy += food.GEnergy;

            Program.DestroyGameObject(food);
        }

    }
}
