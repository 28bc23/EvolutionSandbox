namespace EvolutionSandbox
{
    internal class FoodManager : GameObject
    {
        uint MaxFood;
        float SpawnRate;
        double SpawnAccumulator = 0;

        List<Food> foods = new List<Food>();

        public FoodManager(Guid id) : base(new Vector2Int(0, 0), id, 'M', GameObjectType.Manager)
        {
            MaxFood = Configuration.Config.MaxFoodInEnv;
            SpawnRate = Configuration.Config.FoodSpawnRate;
        }

        public override void Update()
        {
            if (foods.Count >= MaxFood)
            {
                SpawnAccumulator = 0;
                return;
            }

            SpawnAccumulator += SpawnRate * Program.FixedDeltaTime;

            while (SpawnAccumulator >= 1.0 && foods.Count < MaxFood)
            {
                foods.RemoveAll(x => x == null);
                Vector2Int gridSize = Grid.GridSize;
                Vector2Int pos = new Vector2Int(Random.Next(gridSize.X), Random.Next(gridSize.Y));
                Food temp = new Food(pos, Guid.NewGuid());
                if (Program.SpawnGameObject(temp))
                {
                    foods.Add(temp);
                }
                SpawnAccumulator -= 1.0;
            }
        }
    }
}