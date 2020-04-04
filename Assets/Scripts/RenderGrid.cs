using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RenderGrid : MonoBehaviour
{
    public float updateFrequency = 1f;
    public int gridHorizontalUnits = 50;
    public int gridVerticalUnits = 50;

    public GameObject predator;
    public GameObject prey;
    public GameObject vegetable;

    private CELL_TYPE[,] grid;

    private float cellSize;

    Dictionary<CELL_TYPE, GameObject> cellTypeDict;

    void Start()
    {
        grid = new CELL_TYPE[gridHorizontalUnits, gridVerticalUnits];

        grid[0, 1] = CELL_TYPE.PREY;
        grid[1, 2] = CELL_TYPE.PREY;
        grid[2, 0] = CELL_TYPE.PREY;
        grid[2, 1] = CELL_TYPE.PREY;
        grid[2, 2] = CELL_TYPE.PREY;

        cellSize = (Camera.main.orthographicSize * 2) / gridVerticalUnits;

        cellTypeDict = new Dictionary<CELL_TYPE, GameObject> {
            { CELL_TYPE.PREDATOR, predator },
            { CELL_TYPE.PREY, prey },
            { CELL_TYPE.VEGETABLE, vegetable }
        };

        draw();
        InvokeRepeating("step", updateFrequency, updateFrequency);
    }

    void step()
    {
        foreach (GameObject cell in GameObject.FindGameObjectsWithTag("cell")) Destroy(cell);
        grid = GameRules.step(grid);
        draw();
    }

    void draw()
    {
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                spawnCell(grid[i, j], i, j);
            }
        }
    }

    void spawnCell(CELL_TYPE type, int x, int y)
    {
        if (!cellTypeDict.ContainsKey(type)) return;

        Vector3 cellPosition = new Vector3(
            (-cellSize * gridHorizontalUnits / 2) + x * cellSize,
            (cellSize * gridHorizontalUnits / 2) - y * cellSize - cellSize / 2,
            0
        );
        Instantiate(cellTypeDict[type], cellPosition, Quaternion.identity);
    }
}
