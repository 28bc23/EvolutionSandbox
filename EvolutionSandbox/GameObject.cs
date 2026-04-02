using System;
using System.Collections.Generic;

namespace EvolutionSandbox
{
    internal abstract class GameObject
    {
        Vector2Int Pos;

        Queue<Action> actions = new Queue<Action>();

        Guid ID;

        char Character;

        GameObjectType GameObjectType;
        double TimePerAction; // How long one action takes in ms
        DateTime nextActionTime;

        public GameObject(Vector2Int spawnPos, Guid id, char character, GameObjectType gameObjectType)
        {
            Pos = spawnPos;
            ID = id;
            Character = character;
            GameObjectType = gameObjectType;
            TimePerAction = 1000 / Program.APS;
            nextActionTime = DateTime.Now.AddMilliseconds(TimePerAction);
            Program.SpawnGameObject(this, Pos);
        }
        public abstract void Update(double deltaTime);

        public virtual void MakeAction(Action action)
        {
            if(DateTime.Now < nextActionTime)
                return;

            actions.Enqueue(action);
            nextActionTime = DateTime.Now.AddMilliseconds(TimePerAction);
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
    }

    internal enum GameObjectType
    {
        Agent,
        Food,
        Manager
    }
}
