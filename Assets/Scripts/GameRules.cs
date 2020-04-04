using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace gamerules
{
    public interface IRules
    {
        void apply(Board board);
    }

    public class GameRules
    {
        protected DictionaryGrid<int> getNeighboursMap(Board board)
        {
            DictionaryGrid<int> neighboursMap = new DictionaryGrid<int>(board.width, board.height);

            foreach (KeyValuePair<int, ICell> pair in board.content)
            {
                int key = pair.Key;

                if (neighboursMap.ContainsKey(key)) 
                    neighboursMap.setRaw(key, neighboursMap.getRaw(key) + 1);
                else
                    neighboursMap.setRaw(key, 1);
            }
            
            return neighboursMap;
        }
    }

    public class HerbivoreRules: GameRules, IRules
    {
        float spawnProb = .00001f;
        float reproductionProb = 0.01f;
        public void apply(Board board)
        {
            // If somehting breaks, take a look here first
            Board newState = (Board) board.getEmptyCopy();
            DictionaryGrid<int> neighboursMap = getNeighboursMap(board);

            foreach (KeyValuePair<int, int> pair in neighboursMap.content)
            {
                if (!board.ContainsKey(pair.Key))
                {
                    Herbivore candidate;
                    float reproductionChance = pair.Value * reproductionProb / 8;
                    if (Random.Range(0f, 1f) < spawnProb) candidate = new Herbivore();
                    if (Random.Range(0f, 1f) < reproductionChance) candidate = new Herbivore();
                }
            }
        }
    }
}










public class OldGameRules : MonoBehaviour
{

    public static CELL_TYPE[,] step(CELL_TYPE[,] grid)
    {
        CELL_TYPE[,] gridCopy = new CELL_TYPE[grid.GetLength(0), grid.GetLength(1)];

        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                gridCopy[i, j] = fallback(selfSpawnVegetable(grid, i, j), grid[i, j]);
                gridCopy[i, j] = fallback(multiplyVegetable(grid, i, j), gridCopy[i, j]);
                gridCopy[i, j] = fallback(spawnPrey(grid, i, j), gridCopy[i, j]);
            }
        }

        return gridCopy;
    }

    private static CELL_TYPE fallback(CELL_TYPE primary, CELL_TYPE fallback) {
        if (primary != CELL_TYPE.PRESERVE)
            return primary;
        else
            return fallback;
    }

    private static int countNeighbours(CELL_TYPE[,] grid, int x, int y, CELL_TYPE type)
    {
        int horizontalLimit = grid.GetLength(0);
        int verticalLimit = grid.GetLength(1);
        int counter = 0;

        for (int i = x - 1; i <= x + 1; i++)
        {
            for (int j = y - 1; j <= y + 1; j++)
            {
                if (x == i && j == y) continue;

                int safeI = getLoopIndex(i, horizontalLimit);
                int safeJ = getLoopIndex(j, verticalLimit);

                if (grid[safeI, safeJ] == type) ++counter;
            }
        }

        return counter;
    }

    private static CELL_TYPE spawnPrey(CELL_TYPE[,] grid, int i, int j)
    {
        int neighbours = countNeighbours(grid, i, j, CELL_TYPE.PREY);
        if (neighbours == 3)
        {
            return CELL_TYPE.PREY;
        }
        else if (neighbours == 2)
        {
            return CELL_TYPE.PRESERVE;
        }

        return (grid[i,j] == CELL_TYPE.PREY)? CELL_TYPE.NULL : CELL_TYPE.PRESERVE;
    }

    private static CELL_TYPE selfSpawnVegetable(CELL_TYPE[,] grid, int i, int j)
    {
        if (grid[i, j] != CELL_TYPE.NULL) 
            return CELL_TYPE.PRESERVE;
        else
            return (Random.Range(0f, 1f) < vegetableSelfSpawnChance)? CELL_TYPE.VEGETABLE : CELL_TYPE.NULL;
    }

    private static CELL_TYPE multiplyVegetable(CELL_TYPE[,] grid, int i, int j)
    {
        if (grid[i, j] == CELL_TYPE.NULL)
        {
            float neighbours = countNeighbours(grid, i, j, CELL_TYPE.VEGETABLE);
            float chance = neighbours * vegetableReproductionChance / 8;
            return (Random.Range(0f, 1f) < chance) ? CELL_TYPE.VEGETABLE : CELL_TYPE.PRESERVE;
        }
        else
            return CELL_TYPE.PRESERVE;
    }
}
