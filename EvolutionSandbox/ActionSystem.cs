using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionSandbox
{
    internal abstract class Action
    {
        Enum ActionType;
        Vector2Int CurrentPos;

        /* Getters and Setters */
        public Enum GSActionType
        {
            get { return ActionType; }
            protected set { ActionType = value; }
        }

        public Vector2Int GSCurrentPos
        {
            get { return CurrentPos; }
            protected set { CurrentPos = value; }
        }
    }

    internal class MoveAction : Action
    {
        public MoveAction(MovementType movementType, Vector2Int startingPos)
        {
            GSActionType = movementType;
            GSCurrentPos = startingPos;
        }
    }


    internal enum MovementType
    {
        Up,
        Down,
        Right,
        Left,
        UpRight,
        DownRight,
        DownLeft,
        UpLeft,
        NoMove
    }
}
