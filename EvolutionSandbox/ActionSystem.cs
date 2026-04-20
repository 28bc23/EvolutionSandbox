namespace EvolutionSandbox
{
    internal abstract class Action
    {
        Enum ActionType = null;
        Vector2Int CurrentPos;
        GameObject Initiator;
        float EnergyCost = 0;

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

        public float GSEnergyCost
        {
            get { return EnergyCost; }
            protected set { EnergyCost = value; }
        }
    }

    internal class MoveAction : Action
    {
        public MoveAction(MovementType movementType, Vector2Int startingPos, GameObject initiator)
        {
            GSActionType = movementType;
            GSCurrentPos = startingPos;
            GSInitiator = initiator;
            GSEnergyCost = 5;
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
