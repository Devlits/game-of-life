using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Consider a grid, where x - horizontal and y - vertical
// Direction of enumeration is right then bottom
public class DictionaryGrid<T>
{
    public int width { get; }
    public int height { get; }
    public System.Collections.Generic.IEnumerable<GridCell<T>> cells { get { return getCells(); } }
    private Dictionary<int, T> content;

    public DictionaryGrid(int width, int height)
    {
        this.width = width;
        this.height = height;
        this.content = new Dictionary<int, T>();
    }

    private int sanitizeAddress (int num, int max)
    {
        num = num % max;
        return (num >= 0) ? num : max + num;
    }

    /// <summary>
    /// Sets a value to a cell described by (x, y) address.
    /// The addressing is resolved as on a surface of a torus
    /// </summary>
    public void set(int x, int y, T val)
    {
        x = sanitizeAddress(x, width);
        y = sanitizeAddress(y, height);

        int address = y * height + x;
        content[address] = val;
    }

    /// <summary>
    /// Gets a value from a cell described by (x, y) address.
    /// The addressing is resolved as on a surface of a torus
    /// </summary>
    public T get(int x, int y)
    {
        x = sanitizeAddress(x, width);
        y = sanitizeAddress(y, height);

        int address = y * height + x;
        return content[address];
    }

    public bool containsKey(int x, int y)
    {
        x = sanitizeAddress(x, width);
        y = sanitizeAddress(y, height);
        int address = y * height + x;

        return content.ContainsKey(address);
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

    public bool containsKeyRaw(int address)
    {
        return content.ContainsKey(address);
    }

    private System.Collections.Generic.IEnumerable<GridCell<T>> getCells()
    {
        foreach(KeyValuePair<int, T> pair in this.content)
        {
            int retX = pair.Key % width;
            int retY = pair.Key / height;

            yield return new GridCell<T>(retX, retY, pair.Value);
        }
    }
}

public class GridCell<Y>
{
    public int x { get; }
    public int y { get; }
    public Y self { get; }

    public GridCell (int x, int y, Y content)
    {
        this.x = x;
        this.y = y;
        this.self = content;
    }
}