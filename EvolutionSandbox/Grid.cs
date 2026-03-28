using System;
namespace EvolutionSandbox
{
    internal static class Grid
    {
        static bool bInicialised = false;
        static Vector2Int GridSize;
        static char[,] Cells;

        static string LastGrid = "";

        public static void Init(Vector2Int gridSize){
            GridSize = gridSize;
            Cells = new char[GridSize.Y, GridSize.X];
            bInicialised = true;
            ClearGrid();
            DrawGrid();
        }

        public static void ClearGrid(){
            if(!bInicialised)
                return;

            for(int i = 0; i < GridSize.Y; i++){
                for (int j = 0; j < GridSize.X; j++){
                    Cells[i,j] = ' ';
                }
            }
        }

        public static void DrawGrid(){
            if(!bInicialised)
                return;

            string grid = "";

            for(int i = 0; i < GridSize.Y; i++){
                for (int j = 0; j < GridSize.X; j++){
                    grid += $"[{Cells[i,j]}]";
                }
                grid += '\n';
            }

            if (!grid.Equals(LastGrid)) //Revrites grid if there was change
            {
                Console.SetCursorPosition(0, 0);
                Console.Write(grid);
                LastGrid = grid;
            }
        }

        public static void SpawnAgent(Vector2Int pos)
        {
            if (!bInicialised)
                return;

            if (pos.Y < 0 || pos.Y >= GridSize.Y || pos.X < 0 || pos.X >= GridSize.X)
                return;

            Cells[pos.Y, pos.X] = '*';
        }

        public static MoveResult MoveAgent(Vector2Int pos, MovementType type)
        {
            if (!bInicialised)
                return MoveResult.GridNotInicialized;

            if (Cells[pos.Y, pos.X] != '*')
                return MoveResult.TriedToMoveEmptySpaceOrFood;

            int newX = pos.X;
            int newY = pos.Y;

            switch (type)
            {
                case MovementType.Up:
                    newY = pos.Y + 1;
                    if (newY >= GridSize.Y)
                        return MoveResult.CollisionWWall;
                    break;

                case MovementType.Down:
                    newY = pos.Y-1;
                    if (newY < 0)
                        return MoveResult.CollisionWWall;
                    break;

                case MovementType.Right:
                    newX = pos.X+1;
                    if (newX >= GridSize.X)
                        return MoveResult.CollisionWWall;
                    break;

                case MovementType.Left:
                    newX = pos.X-1;
                    if (newX < 0)
                        return MoveResult.CollisionWWall;
                    break;

                case MovementType.UpRight:
                    newY = pos.Y+1;
                    newX = pos.X+1;
                    if (newX >= GridSize.X || newY >= GridSize.Y)
                        return MoveResult.CollisionWWall;
                    break;

                case MovementType.DownRight:
                    newY = pos.Y-1;
                    newX = pos.X+1;
                    if (newX >= GridSize.X || newY < 0)
                        return MoveResult.CollisionWWall;
                    break;

                case MovementType.DownLeft:
                    newY = pos.Y-1;
                    newX = pos.X-1;
                    if (newX < 0 || newY < 0)
                        return MoveResult.CollisionWWall;
                    break;

                case MovementType.UpLeft:
                    newY = pos.Y+1;
                    newX = pos.X-1;
                    if (newX < 0 || newY >= GridSize.Y)
                        return MoveResult.CollisionWWall;
                    break;
                case MovementType.NoMove:
                    return MoveResult.NoMove;

                default:
                    return MoveResult.InvalidMovementType;
            }

            Cells[pos.Y, pos.X] = ' ';
            Cells[newY, newX] = '*';

            pos.X = newX;
            pos.Y = newY;

            return MoveResult.Moved;
        }
    }

    internal enum MoveResult
    {
        Moved,
        CollisionWAgent,
        CollisionWFood,
        CollisionWWall,
        TriedToMoveEmptySpaceOrFood,
        GridNotInicialized,
        InvalidMovementType,
        NoMove
    }
}