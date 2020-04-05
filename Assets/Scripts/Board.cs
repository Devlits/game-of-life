using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using gamerules;

public class Board : DictionaryGrid<CELL_TYPE>
{
    public Board (int width, int height) : base(width, height)
    {

    }

    public Board getEmptyCopy()
    {
        return new Board(width, height);
    }

    public Board getCopy()
    {
        Board copy = new Board(width, height);

        foreach(GridCell<CELL_TYPE> cell in this.cells)
            copy.set(cell.x, cell.y, cell.self);

        return copy;
    }


    public Board applyRules(IRules ruleObject)
    {
        return ruleObject.apply(this);
    }
}