using EvolutionSandbox.NeuralNetwork;

namespace EvolutionSandbox
{
    internal class Agent : GameObject, IComparable<Agent>
    {
        NN nn;
        EvolutionManager Manager;

        public int FoodEaten { get; private set; }
        float EnergyDecreaseRate = Configuration.Config.AgentEnergyDecreaseRate;

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
            FoodEaten++;

            Program.DestroyGameObject(collidedGameObject);
        }

        public override void OnDestroy()
        {
            Manager.RemoveFromAliveList(this);
        }

        public float GetScore()
        {
            return FoodEaten + Math.Max(0, (float)Math.Round(Energy, 2));
        }

        public int CompareTo(Agent? compareAgent)
        {
            if (compareAgent == null)
                return 1;

            else
                return this.GetScore().CompareTo(compareAgent.GetScore());
        }
    }
}
