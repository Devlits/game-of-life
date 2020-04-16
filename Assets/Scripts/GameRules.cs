using System;
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
        protected DictionaryGrid<int> getNeighboursMap(Board board, CELL_TYPE type)
        {
            DictionaryGrid<int> neighboursMap = new DictionaryGrid<int>(board.width, board.height);
            foreach (GridCell<CELL_TYPE> item in board.cells)
            {
                if (!neighboursMap.containsKey(item.x, item.y)) {
                    neighboursMap.set(item.x, item.y, 0);
                }
                for (int x = item.x - 1; x <= item.x + 1; x++)
                {
                    for (int y = item.y - 1; y <= item.y + 1; y++)
                    {
                        if (x == item.x && y == item.y || type != item.self) continue;

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
            if ( !prevState.containsKey(randomX, randomY) && UnityEngine.Random.Range(0f, 1f) < spawnProb)
                newState.set(randomX, randomY, CELL_TYPE.VEGETABLE);

            DictionaryGrid<int> neighboursMap = getNeighboursMap(prevState, CELL_TYPE.VEGETABLE);
            foreach(GridCell<int> cell in neighboursMap.cells)
            {
                if (!prevState.containsKey(cell.x, cell.y))
                {
                    CELL_TYPE candidate = CELL_TYPE.NULL;
                    float reproductionChance = cell.self * reproductionProb / 8;
                    if (UnityEngine.Random.Range(0f, 1f) < reproductionChance) candidate = CELL_TYPE.VEGETABLE;
                    if (candidate != CELL_TYPE.NULL) newState.set(cell.x, cell.y, candidate);
                }
            }

            return newState;
        }
    }

    public class PreyRules: GameRules, IRules
    {
        public Board apply(Board prevState)
        {
            Board newState = prevState.getCopy();
            DictionaryGrid<int> neighboursMap = getNeighboursMap(prevState, CELL_TYPE.PREY);
            foreach (GridCell<int> cell in neighboursMap.cells)
            {
                if (cell.self == 3)
                    newState.set(cell.x, cell.y, CELL_TYPE.PREY);
                else if (cell.self > 3 || cell.self < 2)
                    newState.remove(cell.x, cell.y);
            };

            return newState;
        }
    }
}
