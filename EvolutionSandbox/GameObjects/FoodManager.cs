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

        public bool RemoveFoodFromList(Food food)
        {
            return Foods.Remove(food);
        }
    }
}