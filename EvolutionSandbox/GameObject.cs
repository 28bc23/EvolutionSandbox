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

        public GameObject(Vector2Int spawnPos, Guid id, char character, GameObjectType gameObjectType)
        {
            Pos = spawnPos;
            Grid.SpawnGameObject(this, Pos);
            ID = id;
            Character = character;
            GameObjectType = gameObjectType;
        }
        public abstract void Update();

        public virtual void MakeAction(Action action)
        {
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
    }

    internal enum GameObjectType
    {
        Agent,
        Food
    }
}
