using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRules : MonoBehaviour
{
    public static float vegetableSelfSpawnChance = .0001f;

    private static int getLoopIndex(int index, int max)
    {
        index = index % max;
        return (index >= 0) ? index : max + index;
    }
    private static int countNeighbours(CELL_TYPE[,] grid, int x, int y)
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

                if (grid[safeI, safeJ] != CELL_TYPE.NULL) counter++;
            }
        }

        return counter;
    }
    public static CELL_TYPE[,] step(CELL_TYPE[,] grid)
    {
        CELL_TYPE[,] gridCopy = new CELL_TYPE[grid.GetLength(0), grid.GetLength(1)];

        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                gridCopy[i, j] = spawnHerbivore(grid, i, j);
                gridCopy[i, j] = selfSpanPlant(grid, i, j);
            }
        }

        return gridCopy;
    }

    private static CELL_TYPE spawnHerbivore(CELL_TYPE[,] grid, int i, int j)
    {
        int neighbours = countNeighbours(grid, i, j);
        if (neighbours == 3)
        {
            return CELL_TYPE.PREY;
        }
        else if (neighbours == 2)
        {
            return grid[i, j];
        }

        return CELL_TYPE.NULL;
    }

    private static CELL_TYPE selfSpanPlant(CELL_TYPE[,] grid, int i, int j)
    {
        if (grid[i, j] != CELL_TYPE.NULL) 
            return grid[i, j];
        else
            return (Random.Range(0f, 1f) < vegetableSelfSpawnChance)? CELL_TYPE.VEGETABLE : CELL_TYPE.NULL;
    }
}
