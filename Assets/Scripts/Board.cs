using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using gamerules;

public class Board : DictionaryGrid<ICell>
{
    public Board (int width, int height) : base(width, height)
    {

    }
    public void applyRules(IRules ruleObject)
    {
        ruleObject.apply(this);
    }
}