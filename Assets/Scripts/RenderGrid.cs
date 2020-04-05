﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.UI;
// using System;
using gamerules;

public class RenderGrid : MonoBehaviour
{
    private float updateFrequency = 0.1f;
    private int gridHorizontalUnits = 50;
    private int gridVerticalUnits = 50;

    // public Button button0;
    // public InputField inputField;

    public GameObject predator;
    public GameObject prey;
    public GameObject vegetable;

    private Board board;

    private float cellSize;

    Dictionary<CELL_TYPE, GameObject> cellTypeDict;

    void Start()
    {
        board = new Board(gridHorizontalUnits, gridVerticalUnits);
        board.set(2, 2, CELL_TYPE.VEGETABLE);

        cellSize = (Camera.main.orthographicSize * 2) / gridVerticalUnits;

        cellTypeDict = new Dictionary<CELL_TYPE, GameObject> {
            { CELL_TYPE.PREDATOR, predator },
            { CELL_TYPE.PREY, prey },
            { CELL_TYPE.VEGETABLE, vegetable }
        };

        draw();
        InvokeRepeating("step", updateFrequency, updateFrequency);

        // Button btn0 = button0.GetComponent<Button>();
        // InputField input = inputField.GetComponent<InputField>();
        // inputField.gameObject.SetActive(true);

        // btn0.onClick.AddListener(() =>
        // {
        //     inputField.gameObject.SetActive(false);
        //     btn0.gameObject.SetActive(false);
        //     int cellnumbers = Int32.Parse(input.text);
        //     System.Random rnd = new System.Random();

        //     for (int i = 0; i <= cellnumbers; i++)
        //     {
        //         int clump_size = rnd.Next(4, 6);
        //         for (int j = 0; j < clump_size; j++)
        //         {
        //             int x = rnd.Next(0, gridHorizontalUnits);
        //             int y = rnd.Next(0, gridVerticalUnits);
        //             grid[x, y] = CELL_TYPE.PREY;
        //         }
        //     }


        // });
    }


    void step()
    {
        foreach (GameObject cell in GameObject.FindGameObjectsWithTag("cell")) Destroy(cell);
        board = board.applyRules(new VegetableRules());
        draw();
    }

    void draw()
    {
        foreach (GridCell<CELL_TYPE> cell in board.cells)
        {
            spawnCell(cell.x, cell.y, cell.self);
        }
    }

    void spawnCell(int x, int y, CELL_TYPE type)
    {
        Vector3 cellPosition = new Vector3(
            (-cellSize * gridHorizontalUnits / 2) + x * cellSize,
            (cellSize * gridHorizontalUnits / 2) - y * cellSize - cellSize / 2,
            0
        );
        Instantiate(cellTypeDict[type], cellPosition, Quaternion.identity);
    }
}
