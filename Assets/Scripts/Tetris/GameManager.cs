using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public Transform[,] grid;
    public float fallTime = .8f;
    public static int h = 20;
    public NeuralNetwork nn;
    public int moveCounter = 0;
    public int moveScore = 0;
    public int linesDeleted = 0;
    public static int w = 10;
    public bool debug = false;
    private Transform corner;
    public bool over = false;
    public Block current { private get; set; }
    public int currentBlockType;
    internal float[] GetInput()
    {
        float[] ret = new float[10];
        Transform[] children = current.GetComponentsInChildren<Transform>();
        for(int i=0; i < 8; i += 2)
        {
            (ret[i], ret[i+1]) = GetPositionInGrid(children[i/2]);
        }
        ret[8] = GridH();
        ret[9] = (float) currentBlockType;
        float[] heights = GetHeights();
        float[] r = new float[10 + w];
        ret.CopyTo(r, 0);
        heights.CopyTo(r, ret.Length);
        return r;
    }

    public float[] GetHeights()
    {
        float[] ret = new float[w];
        for (int i = h - 1; i >= 0; i--)
        {
            for (int j = 0; j < w; j++)
            {

                if (grid[j, i] != null && ret[j] == 0.0f) ret[j] = i;
            }
        }
        return ret;
    }

    public float GetHoles()
    {
        float counter = 0.0f;
        float[] peaks = GetHeights();
        for(int i = 0; i < peaks.Length; i++)
        {
            if(peaks[i] > 0)
            {
                for(int j = 0; j < peaks[i]; j++)
                {
                    if (grid[i, j] == null) counter++;
                }
            }
        }
        return counter;
    }
    public float GridH()
    {
        for(int i = h - 1; i >= 0; i--)
        {
            for(int j = 0; j < w; j++)
            {

                if (grid[j, i] != null) return i;
            }
        }
        return 0f;
    }

    public float GetBumpines()
    {
        float[] peaks = GetHeights();
        float buff = 0.0f;
        for(int i = 0; i < peaks.Length-1; i++)
        {
            buff += Mathf.Abs(peaks[i] - peaks[i + 1]);
        }
        return buff;
    }
    private void Awake()
    {
        grid = new Transform[w, h];
        corner = GetComponentInChildren<Corner>().transform;
        nn = new NeuralNetwork(10+w, 3, 4, 3);
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        /*string buff = "";
        foreach(var h in GetHeights())
        {
            buff += h + " ";
        }
        Debug.Log(buff);*/
        //Debug.Log(GetHoles());
    }
    public void CheckForLines()
    {
        for (int i = h - 1; i >= 0; i--)
        {
            if (HasLine(i))
            {
                DeleteLine(i);
                RowDown(i);
            }
        }


    }


    bool HasLine(int i)
    {
        for (int j = 0; j < w; j++)
        {
            if (grid[j, i] == null) return false;
        }
        return true;
    }

    void DeleteLine(int i)
    {
        for (int j = 0; j < w; j++)
        {
            Destroy(grid[j, i].gameObject);
            grid[j, i] = null;

        }
        linesDeleted++;
    }

    void RowDown(int i)
    {
        for (int y = i; y < h; y++)
        {
            for (int j = 0; j < w; j++)
            {
                if (grid[j, y] != null)
                {
                    grid[j, y - 1] = grid[j, y];
                    grid[j, y] = null;
                    grid[j, y - 1].transform.position += Vector3.down;
                }
            }
        }
    }
    public (float, float) GetPositionInGrid(Transform teromino)
    {
        int x = Mathf.RoundToInt(teromino.position.x - corner.position.x);
        int y = Mathf.RoundToInt(teromino.position.y - corner.position.y);
        return (x, y);
    }
    public void AddToGrid(Transform tetromino)
    {
        foreach (Transform child in tetromino)
        {
            int x = Mathf.RoundToInt(child.transform.position.x - corner.position.x);
            int y = Mathf.RoundToInt(child.transform.position.y - corner.position.y);
            if (debug)
            {
                Debug.Log(child.transform.position + " x: " + x + " y: " + y + " " + child.gameObject.name + " " + tetromino.name);
            }
            grid[x, y] = child;
        }
        
    }

    public bool ValidMove(Transform tetromino)
    {
        foreach (Transform child in tetromino)
        {
            int x = Mathf.RoundToInt(child.transform.position.x - corner.position.x);
            int y = Mathf.RoundToInt(child.transform.position.y - corner.position.y);
            if (x < 0 || x >= w || y < 0 || y >= h) return false;
            if (grid[x, y] != null) return false;
        }
        return true;
    }

    public void GameOver()
    {
        //Debug.Log("Game" + name +" over");
        over = true;
    }
}
