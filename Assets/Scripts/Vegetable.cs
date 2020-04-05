using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface ICell
{

}

public abstract class Cell : ICell
{
    public CELL_TYPE id;
}

public class Vegetable : ICell
{
    CELL_TYPE id = CELL_TYPE.VEGETABLE;
}