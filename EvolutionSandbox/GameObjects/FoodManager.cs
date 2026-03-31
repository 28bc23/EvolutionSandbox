using System;
using System.Reflection.PortableExecutable;
namespace EvolutionSandbox
{
    internal class FoodManager : GameObject
    {
        int MaxFood = 10;
        List<Food> foods = new List<Food>();
        public FoodManager(Vector2Int spawnPos, Guid id) : base(spawnPos, id, 'M', GameObjectType.Food)
        {
        }

        public override void Update()
        {
            if (foods.Count < MaxFood)
            {
                //TODO: Spawn food
            }
        }
    }
}