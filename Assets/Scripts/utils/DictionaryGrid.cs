using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Consider a grid, where x - horizontal and y - vertical
// Direction of enumeration is right then bottom
public class DictionaryGrid<T>
{
    public int width { get; }
    public int height { get; }
    public Dictionary<int, T> content { get; }

    public DictionaryGrid(int width, int height)
    {
        this.width = width;
        this.height = height;
    }

    public DictionaryGrid<T> getEmptyCopy()
    {
        return new DictionaryGrid<T>(width, height);
    }

    public void set(int x, int y, T val)
    {
        if (x >= width || x < 0 || y >= height || y < 0) return;

        int address = y * height + x;
        content[address] = val;
    }

    public void setRaw(int address, T val)
    {
        if (address < 0 || address >= height * width) return;
        content[address] = val;
    }

    public T getRaw(int address)
    {
        return content[address];
    }

    public bool ContainsKey(int address)
    {
        return content.ContainsKey(address);
    }

    public T get(int x, int y)
    {
        int address = y * height + x;
        return content[address];
    }
}