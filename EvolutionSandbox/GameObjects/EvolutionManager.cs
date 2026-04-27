namespace EvolutionSandbox
{
    internal class EvolutionManager : GameObject
    {
        public EvolutionManager(Guid id) : base(new Vector2Int(0, 0), id, 'M', GameObjectType.Manager)
        {
            StartNew();
        }

        public override void Update()
        {

        }

        void StartNew() // Starts new evolution based on config
        {
            Grid.Init(new Vector2Int((int)Configuration.Config.GridSizeX, (int)Configuration.Config.GridSizeY)); // Initialize size of grid

            for (int i = 0; i < Configuration.Config.NumAgentsToStartWith; i++)
            {
                Agent agent = new Agent(new Vector2Int(Random.Next((int)Configuration.Config.GridSizeX), Random.Next((int)Configuration.Config.GridSizeY)), Guid.NewGuid());
                Program.SpawnGameObject(agent);
            }

            FoodManager foodManager = new FoodManager(Guid.NewGuid());
            Program.SpawnGameObject(foodManager);
        }

        void StartFormCheckpoint() // Starts evolution basaed on checkpoint
        {

        }

        void SaveProgress() // Creates an checkpoint
        {

        }
    }
}