using System;
using System.Collections.Generic;

namespace EvolutionSandbox
{
    internal abstract class GameObject
    {
        Vector2Int Pos;

        List<Action> actions;

        Guid ID;

        public GameObject(Vector2Int spawnPos, Guid id)
        {
            Pos = spawnPos;
            Grid.SpawnAgent(Pos);
            ID = id;
        }
        public abstract void Update();

        public virtual void MakeAction(Action action)
        {
            actions.Add(action);
        }

        public virtual void ClearActions()
        {
            actions.Clear();
        }




        /* Getters and Setters */
        public Vector2Int GSPos
        {
            get { return Pos; }
            protected set { Pos = value; }
        }

        public Guid GID
        {
            get { return ID;  }
        }

        public List<Action> GActions
        {
            get { return new List<Action>(actions); }
        }
    }
}
