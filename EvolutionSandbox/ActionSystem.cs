namespace EvolutionSandbox
{
    internal abstract class Action
    {
        public Enum? ActionType {  get; protected set; }
        public Vector2Int CurrentPos { get; protected set; }
        public GameObject? Initiator { get; protected set; }
        public float EnergyCost { get; protected set; }
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
