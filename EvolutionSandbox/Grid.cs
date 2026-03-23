namespace EvolutionSandbox
{
    static class Grid
    {
        static bool bInicialised = false;
        static int GirdY;
        static int GridX;
        static char[,] Cells;

        static void Init(int gridY, int gridX){
            gridY = gridY;
            GridX = gridX;
            Cells = new char[gridY, GridX];
            bInicialised = true;
            ClearGrid();
        }

        static void ClearGrid(){
            if(!bInicialised)
                return;

            for(int i = 0; i < GirdY; i++){
                for (int j = 0; j < GridX; j++){
                    Cells[i,j] = '\0';
                }
            }
        }

        static void DrawGrid(){
            if(!bInicialised)
                return;

            for(int i = 0; i < GirdY; i++){
                for (int j = 0; j < GridX; j++){
                    Console.Write($"[{Cells[i,j]}]");
                }
                Console.Write('\n');
            }
        }
    }
}