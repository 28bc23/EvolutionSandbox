using System;
namespace EvolutionSandbox
{
    internal class FoodManager : GameObject
    {
        public FoodManager(Vector2Int spawnPos, Guid id) : base(spawnPos, id, 'M', GameObjectType.Food)
        {
        }

        public override void Update()
        {
            
        }
    }
}