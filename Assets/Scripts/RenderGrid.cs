﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

using gamerules;

public class RenderGrid : MonoBehaviour
{
    private float updateFrequency = 0.1f;
    private int gridHorizontalUnits = 50;
    private int gridVerticalUnits = 50;

    public Button button0;
    public InputField inputField;
    public Slider slider0;

    public GameObject predator;
    public GameObject prey;
    public GameObject vegetable;

    private Board board;

    private float cellSize;
    private int GenerateRandomPreys = 0;
    Dictionary<CELL_TYPE, GameObject> renderedCells;

    void Start()
    {
        board = new Board(gridHorizontalUnits, gridVerticalUnits);
        board.set(2, 2, CELL_TYPE.VEGETABLE);

        board.set(10,10, CELL_TYPE.PREY);
        board.set(10,11, CELL_TYPE.PREY);

        cellSize = (Camera.main.orthographicSize * 2) / gridVerticalUnits;

        renderedCells = new Dictionary<CELL_TYPE, GameObject> {
            { CELL_TYPE.PREDATOR, predator },
            { CELL_TYPE.PREY, prey },
            { CELL_TYPE.VEGETABLE, vegetable }
        };

        draw();
        InvokeRepeating("step", updateFrequency, updateFrequency);

        Button btn0 = button0.GetComponent<Button>();
        InputField input = inputField.GetComponent<InputField>();
        inputField.gameObject.SetActive(true);

        btn0.onClick.AddListener(() =>
        {
            inputField.gameObject.SetActive(false);
            btn0.gameObject.SetActive(false);
            GenerateRandomPreys = Int32.Parse(input.text);
        });
    }

    Board randomSpawn(Board prevState, int cellNumbers)
    {
        System.Random rnd = new System.Random();
        Board newState = prevState.getCopy();

        for (int i = 0; i <= cellNumbers; i++)
        {
            int clumpSize = rnd.Next(4, 6);
            for (int j = 0; j < clumpSize; j++)
            {
                int x = rnd.Next(0, gridHorizontalUnits);
                int y = rnd.Next(0, gridVerticalUnits);
                newState.set(x, y, CELL_TYPE.PREY);
            }
        }

        return newState;
    }


    void step()
    {
        foreach (GameObject cell in GameObject.FindGameObjectsWithTag("cell")) Destroy(cell);
        if (GenerateRandomPreys > 0)
        {
            board = randomSpawn(board, GenerateRandomPreys);
            GenerateRandomPreys = 0;
        }
        board = board.applyRules(new VegetableRules());
        board = board.applyRules(new PreyRules());
        draw();
    }

    void draw()
    {
        foreach (GridCell<CELL_TYPE> cell in board.cells)
            spawnCell(cell.x, cell.y, cell.self);
    }

    void spawnCell(int x, int y, CELL_TYPE type)
    {
        Vector3 cellPosition = new Vector3(
            (-cellSize * gridHorizontalUnits / 2) + x * cellSize,
            (cellSize * gridHorizontalUnits / 2) - y * cellSize - cellSize / 2,
            0
        );
        if ( renderedCells.ContainsKey(type) )
            Instantiate(renderedCells[type], cellPosition, Quaternion.identity);
    }
}
