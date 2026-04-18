namespace EvolutionSandbox
{
    internal static class Grid
    {
        static bool bInicialised = false;
        static Vector2Int GridSize;
        static List<GameObject>[,] Cells = new List<GameObject>[0, 0];

        static string LastGrid = "";

        static bool DrawIndicator = false;

        public static void Init(Vector2Int gridSize){
            GridSize = gridSize;
            Cells = new List<GameObject>[GridSize.Y, GridSize.X];
            bInicialised = true;
            ClearGrid();
            DrawGrid();
        }

        public static void ClearGrid(){
            if(!bInicialised)
                return;

            for(int i = 0; i < GridSize.Y; i++){
                for (int j = 0; j < GridSize.X; j++){
                    Cells[i,j] = new List<GameObject>();
                }
            }
        }

        public static void DrawGrid(){
            if(!bInicialised)
                return;

            string grid = "";

            for(int i = 0; i < GridSize.Y; i++){
                for (int j = 0; j < GridSize.X; j++){
                    if (Cells[i, j].Count == 0)
                        grid += "[ ]";
                    else
                        grid += $"[{Cells[i, j][0].GCharacter}]";
                }
                grid += '\n';
            }

            if (!grid.Equals(LastGrid)) //Revrites grid if there was change
            {
                Console.SetCursorPosition(0, 0);
                Console.Write(grid);

                if (DrawIndicator)
                {
                    Console.WriteLine("0");
                    DrawIndicator = false;
                }else
                    DrawIndicator = true;

                LastGrid = grid;
            }
        }

        public static bool SpawnGameObject(GameObject gameObject, Vector2Int pos, bool doNotSpawnWhenColliding = true, bool ignoreCollisions = false)
        {
            if (!bInicialised)
                return false;

            if (pos.Y < 0 || pos.Y >= GridSize.Y || pos.X < 0 || pos.X >= GridSize.X)
                return false;
            if(!doNotSpawnWhenColliding)
            {
                if (!ignoreCollisions)
                {
                    if (Cells[pos.Y, pos.X].Count > 0)
                    {
                        foreach (GameObject go in Cells[pos.Y, pos.X])
                        {
                            go.OnCollisionEnter(CollisionType.CollisionGameObject, gameObject); // Notify already spawned go
                            gameObject.OnCollisionEnter(CollisionType.CollisionGameObject, go); // Notify new go
                        }
                    }
                }
                Cells[pos.Y, pos.X].Add(gameObject);
            }
            else
            {
                if (Cells[pos.Y, pos.X].Count > 0)
                    return false;
                Cells[pos.Y, pos.X].Add(gameObject);
            }
            return true;
        }

        public static bool RemoveGameObject(GameObject gameObject)
        {
            return Cells[gameObject.GSPos.Y, gameObject.GSPos.X].Remove(gameObject);
        }

        public static GridResult MoveObjects(Dictionary<Guid, Queue<MoveAction>> goMoveActions)
        {
            if (!bInicialised)
                return GridResult.GridNotInicialized;

            while (goMoveActions.Count > 0)
            {
                foreach (Guid key in goMoveActions.Keys.ToList())
                {
                    if (goMoveActions[key].Count == 0)
                    {
                        goMoveActions.Remove(key);
                        continue;
                    }

                    MoveAction moveAction = goMoveActions[key].Dequeue();

                    Vector2Int pos = moveAction.GSInitiator.GSPos;
                    int newX = pos.X;
                    int newY = pos.Y;
                    switch (moveAction.GSActionType)
                    {
                        case MovementType.Up:
                            newY = pos.Y + 1;
                            if (newY >= GridSize.Y){
                                moveAction.GSInitiator.OnCollisionEnter(CollisionType.CollisionWall);
                                continue;
                            }
                            break;

                        case MovementType.Down:
                            newY = pos.Y - 1;
                            if (newY < 0){
                                moveAction.GSInitiator.OnCollisionEnter(CollisionType.CollisionWall);
                                continue;
                            }
                            break;

                        case MovementType.Right:
                            newX = pos.X + 1;
                            if (newX >= GridSize.X){
                                moveAction.GSInitiator.OnCollisionEnter(CollisionType.CollisionWall);
                                continue;
                            }
                            break;

                        case MovementType.Left:
                            newX = pos.X - 1;
                            if (newX < 0){
                                moveAction.GSInitiator.OnCollisionEnter(CollisionType.CollisionWall);
                                continue;
                            }
                            break;

                        case MovementType.JumpUp:
                            newY = pos.Y + 2;
                            if (newY >= GridSize.Y)
                            {
                                moveAction.GSInitiator.OnCollisionEnter(CollisionType.CollisionWall);
                                newY = GridSize.Y - 1;
                            }
                            break;

                        case MovementType.JumpDown:
                            newY = pos.Y - 2;
                            if (newY < 0)
                            {
                                moveAction.GSInitiator.OnCollisionEnter(CollisionType.CollisionWall);
                                newY = 0;
                            }
                            break;

                        case MovementType.JumpRight:
                            newX = pos.X + 2;
                            if (newX >= GridSize.X)
                            {
                                moveAction.GSInitiator.OnCollisionEnter(CollisionType.CollisionWall);
                                newX = GridSize.X-1;
                            }
                            break;

                        case MovementType.JumpLeft:
                            newX = pos.X - 2;
                            if (newX < 0)
                            {
                                moveAction.GSInitiator.OnCollisionEnter(CollisionType.CollisionWall);
                                newX = 0;
                            }
                            break;

                        case MovementType.UpRight:
                            newY = pos.Y + 1;
                            newX = pos.X + 1;
                            if (newX >= GridSize.X || newY >= GridSize.Y){
                                moveAction.GSInitiator.OnCollisionEnter(CollisionType.CollisionWall);
                                continue;
                            }
                            break;

                        case MovementType.DownRight:
                            newY = pos.Y - 1;
                            newX = pos.X + 1;
                            if (newX >= GridSize.X || newY < 0){
                                moveAction.GSInitiator.OnCollisionEnter(CollisionType.CollisionWall);
                                continue;
                            }
                            break;

                        case MovementType.DownLeft:
                            newY = pos.Y - 1;
                            newX = pos.X - 1;
                            if (newX < 0 || newY < 0){
                                moveAction.GSInitiator.OnCollisionEnter(CollisionType.CollisionWall);
                                continue;
                            }
                            break;

                        case MovementType.UpLeft:
                            newY = pos.Y + 1;
                            newX = pos.X - 1;
                            if (newX < 0 || newY >= GridSize.Y){
                                moveAction.GSInitiator.OnCollisionEnter(CollisionType.CollisionWall);
                                continue;
                            }
                            break;
                        default:
                            return GridResult.InvalidMovementType;
                    }
                    Cells[pos.Y, pos.X].Remove(moveAction.GSInitiator);

                    moveAction.GSInitiator.GSPos = new Vector2Int(newX, newY);

                    if (Cells[newY, newX].Count > 0)
                    {
                        foreach(GameObject go in Cells[newY, newX].ToArray())
                        {
                            moveAction.GSInitiator.OnCollisionEnter(CollisionType.CollisionGameObject, go);
                            go.OnCollisionEnter(CollisionType.CollisionGameObject, moveAction.GSInitiator);
                        }
                    }
                    Cells[newY, newX].Add(moveAction.GSInitiator);

                    

                    if (goMoveActions[key].Count == 0)
                        goMoveActions.Remove(key);
                }
            }

            return GridResult.Success;
        }

        /* Getters ans Setters */
        public static Vector2Int GGridSize
        {
            get {  return GridSize; }
        }
    }

    internal enum GridResult
    {
        Success,
        TriedToMoveEmptySpaceOrFood,
        GridNotInicialized,
        InvalidMovementType,
    }

    internal enum CollisionType
    {
        CollisionGameObject,
        CollisionWall,
    }
}