using EvolutionSandbox.NeuralNetwork;

namespace EvolutionSandbox
{
    internal class Agent : GameObject
    {
        float EnergyDecreaseRate = Configuration.Config.AgentEnergyDecreaseRate;
        NN nn;
        EvolutionManager Manager;
        public Agent(Vector2Int spawnPos, Guid id, EvolutionManager manager) : base(spawnPos, id, '*', GameObjectType.Agent, Configuration.Config.AgentMaxEnergy)
        {
            nn = new NN(5, 13);
            Manager = manager;
        }

        public override void Update()
        {
            //Decrease energy
            Energy -= EnergyDecreaseRate * Program.FixedDeltaTime;
            if (Energy <= 0)
            {
                Program.DestroyGameObject(this);
                return;
            }

            //Generate random input for forward pass
            double[] input = new double[nn.InputSize];
            for (int i = 0; i < input.Length; i++)
            {
                input[i] = Random.NextDouble(-1, 1);
            }

            MovementType move = nn.Forward(input);
            MakeAction(new MoveAction(move, Pos, this));
        }

        public override void OnCollisionEnter(CollisionType collision, GameObject collidedGameObject)
        {
            if (collision != CollisionType.CollisionGameObject)
                return;

            if (collidedGameObject.GameObjectType != GameObjectType.Food)
                return;

            Energy += collidedGameObject.Energy; // collidedGameObject should be food thanks to if statement above

            Program.DestroyGameObject(collidedGameObject);
        }

        public override void OnDestroy()
        {
            Manager.RemoveFromAliveList(this);
        }
    }
}
