using System;
namespace EvolutionSandbox
{
    static class Grid
    {
        static bool bInicialised = false;
        static int GridY;
        static int GridX;
        static char[,] Cells;

        static string LastGrid = "";

        public static void Init(int gridY, int gridX){
            GridY = gridY;
            GridX = gridX;
            Cells = new char[gridY, GridX];
            bInicialised = true;
            ClearGrid();
        }

        public static void ClearGrid(){
            if(!bInicialised)
                return;

            for(int i = 0; i < GridY; i++){
                for (int j = 0; j < GridX; j++){
                    Cells[i,j] = ' ';
                }
            }
        }

        public static void DrawGrid(){
            if(!bInicialised)
                return;

            string grid = "";

            for(int i = 0; i < GridY; i++){
                for (int j = 0; j < GridX; j++){
                    grid += $"[{Cells[i,j]}]";
                }
                grid += '\n';
            }

            if (!grid.Equals(LastGrid))
            {
                Console.Clear();
                Console.Write(grid);
                LastGrid = grid;
            }            
        }

        public static void SpawnAgent(int y, int x)
        {
            //DEBUG
            Cells[y, x] = '*';
            Cells[y+1, x] = 'X';
            DrawGrid();
        }
    }
}