using static EvolutionSandbox.Utils.Configuration;

namespace EvolutionSandbox.GameObjects
{
    internal class Water : GameObject
    {
        public Water(Vector2Int pos, Guid id) : base(pos, id, Config.WaterCharacter, GameObjectType.Water, Config.WaterColor, 0)
        {

        }
    }
}