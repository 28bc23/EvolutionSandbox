namespace EvolutionSandbox
{
    internal class EvolutionManager : GameObject
    {
        public EvolutionManager(Guid id) : base(new Vector2Int(0, 0), id, 'M', GameObjectType.Manager)
        {

        }

        public override void Update()
        {

        }

        void StartNew() // Starts new evolution based on config
        {

        }

        void StartFormCheckpoint() // Starts evolution basaed on checkpoint
        {

        }

        void SaveProgress() // Creates an checkpoint
        {

        }
    }
}