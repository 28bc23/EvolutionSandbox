using System;
using System.Collections.Generic;
namespace EvolutionSandbox
{
    internal class FoodManager : GameObject
    {
        int MaxFood = 10;
        List<Food> foods = new List<Food>();
        public FoodManager(Guid id) : base(new Vector2Int(0, 0), id, 'M', GameObjectType.Manager)
        {
        }

        public override void Update()
        {
            if (foods.Count < MaxFood)
            {
                Vector2Int gridSize = Grid.GGridSize;
                Vector2Int pos = new Vector2Int(Program.GRND.Next(0, gridSize.X - 1), Program.GRND.Next(0, gridSize.Y - 1));
                foods.Add(new Food(pos, Guid.NewGuid()));
            }
        }
    }
}