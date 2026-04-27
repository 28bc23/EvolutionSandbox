namespace EvolutionSandbox
{
    internal abstract class GameObject
    {
        public Vector2Int Pos { get; set; }

        Queue<Action> actions = new Queue<Action>();

        public Guid ID { get; private set; }

        public char Character { get; private set; }

        public GameObjectType GameObjectType { get; private set; }

        public double Energy { get; protected set; }

        public GameObject(Vector2Int spawnPos, Guid id, char character, GameObjectType gameObjectType, float energy = 0)
        {
            Pos = spawnPos;
            ID = id;
            Character = character;
            GameObjectType = gameObjectType;
            Energy = energy;
        }
        public abstract void Update();

        public virtual void MakeAction(Action action)
        {
            if (GameObjectType == GameObjectType.Agent)
                Energy -= action.EnergyCost;

            actions.Enqueue(action);
        }

        public virtual void ClearActions()
        {
            actions.Clear();
        }

        public virtual void OnCollisionEnter(CollisionType collision)
        {
            return;
        }

        public virtual void OnCollisionEnter(CollisionType collision, GameObject collidedGameObject)
        {
            return;
        }

        public virtual void OnDestroy()
        {

        }

        public Queue<Action> GetCopyOfActions()
        {
            return new Queue<Action>(actions);
        }
    }

    internal enum GameObjectType
    {
        Agent,
        Food,
        Manager
    }
}
