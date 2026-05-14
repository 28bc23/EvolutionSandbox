using EvolutionSandbox.Utils;

namespace EvolutionSandbox.GameObjects
{
    internal class Food : GameObject
    {
        FoodManager Manager;
        public Food(Vector2Int spawnPos, Guid id, FoodManager manager) : base(spawnPos, id, Configuration.Config.FoodCharacter,
        GameObjectType.Food, Configuration.Config.FoodColor, Configuration.Config.FoodEnergy)
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