using EvolutionSandbox;

namespace EvolutionSandbox.GameObjects
{
    internal abstract class GameObject : IComparable<GameObject>
    {
        public Vector2Int Pos { get; set; }

        Queue<Action> actions = new Queue<Action>();

        public Guid ID { get; set; }

        public char Character { get; private set; }

        public GameObjectType GameObjectType { get; private set; }

        public double Energy { get; protected set; }

        public ConsoleColor Color { get; protected set; }

        public GameObject(Vector2Int spawnPos, Guid id, char character, GameObjectType gameObjectType, ConsoleColor color, float energy = 0)
        {
            Pos = spawnPos;
            ID = id;
            Character = character;
            GameObjectType = gameObjectType;
            Color = color;
            Energy = energy;
        }
        public virtual void Update()
        {

        }

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

        public int CompareTo(GameObject? compareGO)
        {
            if (compareGO == null)
                return 1;

            int typeCompare = this.GameObjectType.CompareTo(compareGO.GameObjectType);
            if (typeCompare != 0)
                return typeCompare;

            return this.ID.CompareTo(compareGO.ID);
        }
    }

    internal enum GameObjectType
    {
        Agent,
        Food,
        Water,
        Manager
    }
}
