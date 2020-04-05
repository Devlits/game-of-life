using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace gamerules
{
    public interface IRules
    {
        Board apply(Board board);
    }

    public class GameRules
    {
        protected DictionaryGrid<int> getNeighboursMap(Board board)
        {
            DictionaryGrid<int> neighboursMap = new DictionaryGrid<int>(board.width, board.height);

            foreach (GridCell<CELL_TYPE> item in board.cells)
            {
                for (int x = item.x - 1; x <= item.x + 1; x++)
                {
                    for (int y = item.y - 1; y <= item.y + 1; y++)
                    {
                        if (x == item.x && y == item.y) continue;

                        if (neighboursMap.containsKey(x, y))
                            neighboursMap.set(x, y, neighboursMap.get(x, y) + 1);
                        else
                            neighboursMap.set(x, y, 1);
                    }
                }
            }

            return neighboursMap;
        }
    }

    public class VegetableRules: GameRules, IRules
    {
        float spawnProb = .001f;
        float reproductionProb = .01f;
        public Board apply(Board prevState)
        {
            Board newState = prevState.getCopy();

            System.Random rnd = new System.Random();
            int randomX = rnd.Next(prevState.width);
            int randomY = rnd.Next(prevState.height);
            if ( !prevState.containsKey(randomX, randomY) && Random.Range(0f, 1f) < spawnProb)
                newState.set(randomX, randomY, CELL_TYPE.VEGETABLE);


            DictionaryGrid<int> neighboursMap = getNeighboursMap(prevState);
            foreach(GridCell<int> cell in neighboursMap.cells)
            {
                if (!prevState.containsKey(cell.x, cell.y))
                {
                    CELL_TYPE candidate = CELL_TYPE.NULL;
                    float reproductionChance = cell.self * reproductionProb / 8;
                    if (Random.Range(0f, 1f) < reproductionChance) candidate = CELL_TYPE.VEGETABLE;
                    if (candidate != CELL_TYPE.NULL) newState.set(cell.x, cell.y, candidate);
                }
            }

            

            return newState;
        }
    }
}










// public class OldGameRules : MonoBehaviour
// {

//     public static CELL_TYPE[,] step(CELL_TYPE[,] grid)
//     {
//         CELL_TYPE[,] gridCopy = new CELL_TYPE[grid.GetLength(0), grid.GetLength(1)];

//         for (int i = 0; i < grid.GetLength(0); i++)
//         {
//             for (int j = 0; j < grid.GetLength(1); j++)
//             {
//                 gridCopy[i, j] = fallback(selfSpawnVegetable(grid, i, j), grid[i, j]);
//                 gridCopy[i, j] = fallback(multiplyVegetable(grid, i, j), gridCopy[i, j]);
//                 gridCopy[i, j] = fallback(spawnPrey(grid, i, j), gridCopy[i, j]);
//             }
//         }

//         return gridCopy;
//     }

//     private static CELL_TYPE fallback(CELL_TYPE primary, CELL_TYPE fallback) {
//         if (primary != CELL_TYPE.PRESERVE)
//             return primary;
//         else
//             return fallback;
//     }

//     private static int countNeighbours(CELL_TYPE[,] grid, int x, int y, CELL_TYPE type)
//     {
//         int horizontalLimit = grid.GetLength(0);
//         int verticalLimit = grid.GetLength(1);
//         int counter = 0;

//         for (int i = x - 1; i <= x + 1; i++)
//         {
//             for (int j = y - 1; j <= y + 1; j++)
//             {
//                 if (x == i && j == y) continue;

//                 int safeI = getLoopIndex(i, horizontalLimit);
//                 int safeJ = getLoopIndex(j, verticalLimit);

//                 if (grid[safeI, safeJ] == type) ++counter;
//             }
//         }

//         return counter;
//     }

//     private static CELL_TYPE spawnPrey(CELL_TYPE[,] grid, int i, int j)
//     {
//         int neighbours = countNeighbours(grid, i, j, CELL_TYPE.PREY);
//         if (neighbours == 3)
//         {
//             return CELL_TYPE.PREY;
//         }
//         else if (neighbours == 2)
//         {
//             return CELL_TYPE.PRESERVE;
//         }

//         return (grid[i,j] == CELL_TYPE.PREY)? CELL_TYPE.NULL : CELL_TYPE.PRESERVE;
//     }

//     private static CELL_TYPE selfSpawnVegetable(CELL_TYPE[,] grid, int i, int j)
//     {
//         if (grid[i, j] != CELL_TYPE.NULL) 
//             return CELL_TYPE.PRESERVE;
//         else
//             return (Random.Range(0f, 1f) < vegetableSelfSpawnChance)? CELL_TYPE.VEGETABLE : CELL_TYPE.NULL;
//     }

//     private static CELL_TYPE multiplyVegetable(CELL_TYPE[,] grid, int i, int j)
//     {
//         if (grid[i, j] == CELL_TYPE.NULL)
//         {
//             float neighbours = countNeighbours(grid, i, j, CELL_TYPE.VEGETABLE);
//             float chance = neighbours * vegetableReproductionChance / 8;
//             return (Random.Range(0f, 1f) < chance) ? CELL_TYPE.VEGETABLE : CELL_TYPE.PRESERVE;
//         }
//         else
//             return CELL_TYPE.PRESERVE;
//     }
// }
