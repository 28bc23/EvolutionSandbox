namespace EvolutionSandbox
{
    internal class FoodManager : GameObject
    {
        uint MaxFood;
        float SpawnRate;
        double SpawnAccumulator = 0;

        List<Food> Foods = new List<Food>();

        public FoodManager(Guid id) : base(new Vector2Int(0, 0), id, 'M', GameObjectType.Manager)
        {
            MaxFood = Configuration.Config.MaxFoodInEnv;
            SpawnRate = Configuration.Config.FoodSpawnRate;
        }

        public override void Update()
        {
            if (Foods.Count >= MaxFood)
            {
                SpawnAccumulator = 0;
                return;
            }

            SpawnAccumulator += SpawnRate * Program.FixedDeltaTime;

            while (SpawnAccumulator >= 1.0 && Foods.Count < MaxFood)
            {
                Vector2Int gridSize = Grid.GridSize;
                Vector2Int pos = new Vector2Int(Random.Next(gridSize.X), Random.Next(gridSize.Y));
                Food temp = new Food(pos, Guid.NewGuid(), this);
                if (Program.SpawnGameObject(temp))
                {
                    Foods.Add(temp);
                }
                SpawnAccumulator -= 1.0;
            }
        }

        public Vector2Int GetPosOfClosestFood(Vector2Int pos)
        {
            float minDistance = float.MaxValue;
            Vector2Int closestPos = new Vector2Int(-1, -1); // this position shouldn't exist on grid so if we feed it to NN it can learn that propably no food is around
            foreach (Food food in Foods)
            {
                float distance = MathF.Pow(food.Pos.X - pos.X, 2) + MathF.Pow(food.Pos.Y - pos.Y, 2); // we won't do sqrt to make it faster. we don't nessery need it here anyway
                if(distance < minDistance)
                {
                    minDistance = distance;
                    closestPos = food.Pos;
                }
            }

            return closestPos;
        }

        public bool RemoveFoodFromList(Food food)
        {
            return Foods.Remove(food);
        }
    }
}