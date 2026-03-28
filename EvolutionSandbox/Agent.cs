using System;

namespace EvolutionSandbox
{
    internal class Agent : GameObject
    {
        Random rnd;
        public Agent(Vector2Int spawnPos, Guid id) : base(spawnPos, id)
        {
            rnd = new Random();
        }

        public override void Update() //All actions in here will be made in one frame. Need to find way to work with it. but i will propably be doing one action most of the time. Else I cloud use maybe corutine and timeDelta time.
        {
            //Movement test
            MovementType randomMove = (MovementType)rnd.Next(0, 8);
            MakeAction(new MoveAction(randomMove, GSPos));
        }

    }
}
