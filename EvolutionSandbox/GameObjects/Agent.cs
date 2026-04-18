using EvolutionSandbox.NeuralNetwork;

namespace EvolutionSandbox
{
    internal class Agent : GameObject
    {
        float EnergyDecreaseRate = 1;
        NN nn;
        public Agent(Vector2Int spawnPos, Guid id) : base(spawnPos, id, '*', GameObjectType.Agent, 100)
        {
            nn = new NN(5, 13);
        }

        public override void Update()
        {
            //Desrease energy
            GSEnergy -= EnergyDecreaseRate * Program.GDeltaTime;
            if (GSEnergy <= 0)
            {
                Program.DestroyGameObject(this);
            }

            //Generate random input for forward pass
            double[] input = new double[nn.GInputSize];
            for (int i = 0; i < input.Length; i++)
            {
                input[i] = Random.RandomRangeDouble(-1, 1);
            }

            MovementType move = nn.Forward(input);
            MakeAction(new MoveAction(move, GSPos, this));
        }

        public override void OnCollisionEnter(CollisionType collision, GameObject collidedGameObject)
        {
            if (collision != CollisionType.CollisionGameObject)
                return;

            if (collidedGameObject.GGameObjectType != GameObjectType.Food)
                return;

            GSEnergy += collidedGameObject.GSEnergy; // collidedGameObject should be food thanks to if statement above

            Program.DestroyGameObject(collidedGameObject);
        }

    }
}
