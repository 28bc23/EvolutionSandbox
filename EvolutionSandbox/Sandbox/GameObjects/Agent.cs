using System.Diagnostics;
using EvolutionSandbox;
using EvolutionSandbox.NeuralNetwork;
using EvolutionSandbox.Utils;
using static EvolutionSandbox.Utils.Configuration;

namespace EvolutionSandbox.GameObjects
{
    internal class Agent : GameObject, IComparable<Agent>
    {
        NN nn;
        EvolutionManager Manager;

        public int FoodEaten { get; private set; }

        double MaxEnergy = Config.AgentMaxEnergy;

        public Agent(Vector2Int spawnPos, Guid id, EvolutionManager manager, bool initializeNN = true) : base(spawnPos, id, Config.AgentCharacter, GameObjectType.Agent, Config.AgentColor, energy: Config.AgentMaxEnergy)
        {
            nn = new NN(7, 13, initializeNN);
            Manager = manager;
        }

        public override void Update()
        {
            //Decrease energy
            Energy -= Config.AgentEnergyDecreaseRate * Program.FixedDeltaTime;
            if (Energy <= 0)
            {
                Program.DestroyGameObject(this);
                return;
            }

            Vector2Int closestFoodPos = Manager.GetPosOfClosestFood(Pos);

            double[] input = new double[nn.InputSize];
            input[0] = (Pos.X - closestFoodPos.X) / (double)Grid.GridSize.X; // x direction to food
            input[1] = (Pos.Y - closestFoodPos.Y) / (double)Grid.GridSize.Y; // y direction to food
            input[2] = (Energy / MaxEnergy) * 2.0 - 1.0; // current energy
            input[3] = Pos.Y / (double)Grid.GridSize.Y; // distance from bottom edge
            input[4] = (Grid.GridSize.Y - Pos.Y) / (double)Grid.GridSize.Y; // distance from upper edge
            input[5] = Pos.X / (double)Grid.GridSize.X; // distance from left edge
            input[6] = (Grid.GridSize.X - Pos.X) / (double)Grid.GridSize.X; // distance from right edge

            MovementType move = nn.Forward(input);
            MakeAction(new MoveAction(move, Pos, this));
        }

        public override void OnCollisionEnter(CollisionType collision, GameObject collidedGameObject)
        {

            if (collision == CollisionType.CollisionGameObject)
            {
                if (collidedGameObject.GameObjectType == GameObjectType.Food)
                {
                    Energy += collidedGameObject.Energy; // collidedGameObject should be food thanks to if statement above
                    FoodEaten++;

                    Program.DestroyGameObject(collidedGameObject);
                }
                else if (collidedGameObject.GameObjectType == GameObjectType.Water)
                {
                }
            }

            base.OnCollisionEnter(collision, collidedGameObject);
        }

        public override void OnCollisionEnter(CollisionType collision)
        {
            if (collision == CollisionType.CollisionWall)
            {
                Energy -= Config.AgentWallCollisionEnergyPenalty;
            }
            base.OnCollisionEnter(collision);
        }

        public override void OnCollisionExit(CollisionType collision, GameObject collidedGameObject)
        {
            if (collision == CollisionType.CollisionGameObject)
            {
                if (collidedGameObject.GameObjectType == GameObjectType.Water)
                {
                }
            }

            base.OnCollisionExit(collision, collidedGameObject);
        }

        public override void OnDestroy()
        {
            Manager.RemoveFromAliveList(this);
        }

        public float GetScore()
        {
            float energyBonus = (float)(Energy - MaxEnergy) / (float)MaxEnergy;

            Vector2Int closestFoodPos = Manager.GetPosOfClosestFood(Pos);
            float x = MathF.Pow(Pos.X - closestFoodPos.X, 2);
            float y = MathF.Pow(Pos.Y - closestFoodPos.Y, 2);
            float diagonal = MathF.Sqrt(MathF.Pow(Grid.GridSize.X, 2) + MathF.Pow(Grid.GridSize.Y, 2));
            float closeToFoodBonus = (diagonal - MathF.Sqrt(x + y)) / diagonal;

            x = Pos.X / (float)Grid.GridSize.X;
            y = Pos.Y / (float)Grid.GridSize.Y;
            float centerBonus = 1 - MathF.Abs(x - 0.5f) - MathF.Abs(y - 0.5f); // next to wall == 0; in center = 1;

            return Config.FoodScoreCoef * FoodEaten + Config.EnergyBonusCoef * energyBonus + Config.CloseToFoodBonusCoef * closeToFoodBonus + Config.CenterBonusCoef * centerBonus;
        }

        public Agent DeepCopy(bool mutate = true)
        {
            Agent agent = new Agent(new Vector2Int(Utils.Random.Next((int)Config.GridSizeX),
                  Utils.Random.Next((int)Config.GridSizeY)),
                Utils.Random.NextGuid(), Manager, false);
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

        public NN GetNNCopy()
        {
            return nn.Copy(false);
        }

        public void SetNN(NN nn, bool mutate)
        {
            this.nn = nn.Copy(mutate);
        }
    }
}
