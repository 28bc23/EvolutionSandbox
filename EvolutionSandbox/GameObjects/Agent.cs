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
            nn = new NN(7, 13);
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

            Vector2Int closestFoodPos = Manager.GetPosOfClosestFood(Pos);

            double[] input = new double[nn.InputSize];
            input[0] = (Pos.X - closestFoodPos.X) / (double)Grid.GridSize.X; // x direction to food
            input[1] = (Pos.Y - closestFoodPos.Y) / (double)Grid.GridSize.Y; // y direction to food
            input[2] = (Energy / Configuration.Config.AgentMaxEnergy) * 2.0 - 1.0; // current energy
            input[3] = Pos.Y / (double)Grid.GridSize.Y; // distance from bottom edge
            input[4] = (Grid.GridSize.Y - Pos.Y) / (double)Grid.GridSize.Y; // distance from upper edge
            input[5] = Pos.X / (double)Grid.GridSize.X; // distance from left edge
            input[6] = (Grid.GridSize.X - Pos.X) / (double)Grid.GridSize.X; // distance from right edge

            MovementType move = nn.Forward(input);
            MakeAction(new MoveAction(move, Pos, this));
        }

        public override void OnCollisionEnter(CollisionType collision, GameObject collidedGameObject)
        {
            if(collision == CollisionType.CollisionWall)
            {
                Energy -= Configuration.Config.AgentWallCollisionEnergyPenalty;
            }

            if (collision == CollisionType.CollisionGameObject)
            {
                if (collidedGameObject.GameObjectType == GameObjectType.Food)
                {
                    Energy += collidedGameObject.Energy; // collidedGameObject should be food thanks to if statement above
                    FoodEaten++;

                    Program.DestroyGameObject(collidedGameObject);
                }
            }
        }

        public override void OnDestroy()
        {
            Manager.RemoveFromAliveList(this);
        }

        public float GetScore()
        {
            return FoodEaten;
        }

        public Agent DeepCopy(bool mutate = true)
        {
            Agent agent = new Agent(new Vector2Int(Random.Next((int)Configuration.Config.GridSizeX),
                    Random.Next((int)Configuration.Config.GridSizeY)),
                    Guid.NewGuid(), Manager);
            agent.nn = nn.Copy(mutate);
            return agent;
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
