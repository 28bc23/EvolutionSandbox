namespace EvolutionSandbox
{
    internal class Food : GameObject
    {
        
        public Food(Vector2Int spawnPos, Guid id) : base(spawnPos, id, 'X', GameObjectType.Food, Configuration.Config.FoodEnergy)
        {
            
        }

        public override void Update()
        {
            
        }
    }
}