namespace EvolutionSandbox
{
    internal abstract class GameObject
    {
        Vector2Int Pos;

        Queue<Action> actions = new Queue<Action>();

        Guid ID;

        char Character;

        GameObjectType GameObjectType;

        double Energy = 10;

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
                Energy -= action.GSEnergyCost;

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




        /* Getters and Setters */
        public Vector2Int GSPos
        {
            get { return Pos; }
            set { Pos = value; }
        }

        public Guid GID
        {
            get { return ID;  }
        }

        public Queue<Action> GActions
        {
            get { return new Queue<Action>(actions); }
        }

        public char GCharacter
        {
            get { return Character; }
        }

        public GameObjectType GGameObjectType
        {
            get { return GameObjectType; }
        }

        public double GSEnergy
        {
            get { return Energy; }
            protected set { Energy = value; }
        }
    }

    internal enum GameObjectType
    {
        Agent,
        Food,
        Manager
    }
}
