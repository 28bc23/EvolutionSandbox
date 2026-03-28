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
        GameObject Initiator;

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

        public GameObject GSInitiator
        {
            get { return Initiator; }
            protected set { Initiator = value; }
        }
    }

    internal class MoveAction : Action
    {
        public MoveAction(MovementType movementType, Vector2Int startingPos, GameObject initiator)
        {
            GSActionType = movementType;
            GSCurrentPos = startingPos;
            GSInitiator = initiator;
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
        NoMove,
        JumpUp,
        JumpDown,
        JumpLeft,
        JumpRight
    }
}
