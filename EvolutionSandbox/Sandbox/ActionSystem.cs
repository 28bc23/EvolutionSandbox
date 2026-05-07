using EvolutionSandbox.GameObjects;
using EvolutionSandbox.Utils;

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
            ActionType = movementType;
            CurrentPos = startingPos;
            Initiator = initiator;

            if (movementType == MovementType.NoMove)
                EnergyCost = Configuration.Config.AgentNoActionEnergyCost;
            else if (movementType == MovementType.JumpUp || movementType == MovementType.JumpDown || movementType == MovementType.JumpLeft || movementType == MovementType.JumpRight)
            {
                EnergyCost = Configuration.Config.AgentJumpActionEnergyCost;
            }
            else
            {
                EnergyCost = Configuration.Config.AgentStepActionEnergyCost;
            }
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
