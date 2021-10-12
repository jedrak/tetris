using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform[,] grid;
    public float fallTime = .8f;
    public static int h = 20;
    public static int w = 10;
    public bool debug = false;
    private Transform corner;
    public bool over = false;
    private void Awake()
    {
        grid = new Transform[w, h];
        corner = GetComponentInChildren<Corner>().transform;
    }

    private void Start()
    {
        
    }

    private void Update()
    {

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
        Debug.Log("Game" + name +" over");
        over = true;
    }
}
