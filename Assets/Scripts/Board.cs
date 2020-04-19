using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using gamerules;

public class Board : DictionaryGrid<ActiveCell>
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

        foreach(GridCell<ActiveCell> cell in this.cells)
            copy.set(cell.x, cell.y, cell.self);

        return copy;
    }


    public Board applyRules(IRules ruleObject)
    {
        return ruleObject.apply(this);
    }

    public void setEnv(int x, int y, ENV_TYPE env)
    {
        int address = sanitizeAddress(x, y);
        if (!content.ContainsKey(address))
            set(x, y, new ActiveCell());
        content[address].env = env;
    }

    public void setCell(int x, int y, CELL_TYPE cell)
    {
        int address = sanitizeAddress(x, y);
        if (!content.ContainsKey(address))
            set(x, y, new ActiveCell());
        content[address].cell = cell;
    }

    public ENV_TYPE getEnv(int x, int y)
    {
        int address = sanitizeAddress(x, y);
        return content[address].env;
    }

    public CELL_TYPE getCell(int x, int y)
    {
        int address = sanitizeAddress(x, y);
        return content[address].cell;
    }

    public bool containsEnv(int x, int y)
    {
        int address = sanitizeAddress(x, y);
        if (content.ContainsKey(address))
            return getEnv(x, y) != ENV_TYPE.NULL;
        else
            return false;
    }

    public bool containsCell(int x, int y)
    {
        int address = sanitizeAddress(x, y);
        if (content.ContainsKey(address))
            return getCell(x, y) != CELL_TYPE.NULL;
        else
            return false;
    }

    public void removeEnv(int x, int y)
    {
        if (!containsKey(x, y)) return;

        if (getCell(x, y) != CELL_TYPE.NULL)
        {
            int address = sanitizeAddress(x, y);
            content[address].cell = CELL_TYPE.NULL;
        }
        else remove(x, y);
    }

    public void removeCell(int x, int y)
    {
        if (!containsKey(x, y)) return;

        if (getEnv(x, y) != ENV_TYPE.NULL) {
            int address = sanitizeAddress(x, y);
            content[address].cell = CELL_TYPE.NULL;
        }
        else remove(x, y);
    }
}

public class ActiveCell {
    public ENV_TYPE env {get; set;} = ENV_TYPE.NULL;
    public CELL_TYPE cell {get; set;} = CELL_TYPE.NULL;

    public ActiveCell() {}
    public ActiveCell(ENV_TYPE env) {
        this.env = env;
    }

    public ActiveCell(CELL_TYPE cell) {
        this.cell = cell;
    }

    public ActiveCell(CELL_TYPE cell, ENV_TYPE env){
        this.cell = cell;
        this.env = env;
    }
}