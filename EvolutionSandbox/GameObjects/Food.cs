namespace EvolutionSandbox
{
    internal class Food : GameObject
    {
        public Food(Vector2Int spawnPos, Guid id) : base(spawnPos, id, 'X', GameObjectType.Agent)
        {
        }

        public override void Update()
        {
            
        }
    }
}