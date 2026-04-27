namespace EvolutionSandbox
{
    internal class Food : GameObject
    {
        FoodManager Manager;
        public Food(Vector2Int spawnPos, Guid id, FoodManager manager) : base(spawnPos, id, 'X', GameObjectType.Food, Configuration.Config.FoodEnergy)
        {
            Manager = manager;
        }

        public override void Update()
        {

        }

        public override void OnDestroy()
        {
            Manager.RemoveFoodFromList(this);
        }
    }
}