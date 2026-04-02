using System;
namespace EvolutionSandbox
{
    internal class Food : GameObject
    {
        float Energy = 10;
        public Food(Vector2Int spawnPos, Guid id) : base(spawnPos, id, 'X', GameObjectType.Food)
        {
        }

        public override void Update(double deltaTime)
        {
            
        }

        public float GEnergy
        {
            get { return Energy; }
        }
    }
}