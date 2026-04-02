using System;
namespace EvolutionSandbox
{
    internal class Food : GameObject
    {
        
        public Food(Vector2Int spawnPos, Guid id) : base(spawnPos, id, 'X', GameObjectType.Food, 10)
        {
            
        }

        public override void Update()
        {
            
        }
    }
}