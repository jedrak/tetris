using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    private float previousTime;
    public float fallTime=.8f;
    public static int h = 20;
    public static int w = 10;
    public Vector3 rotationPoint;
    private static Transform[,] grid = new Transform[w, h];
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += (Vector3.left);
            if (!ValidMove()) transform.position += (Vector3.right);
        }else if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += (Vector3.right);
            if (!ValidMove()) transform.position += (Vector3.left);
        }else if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.RotateAround(transform.TransformPoint(rotationPoint), Vector3.forward, 90);
            if(!ValidMove()) transform.RotateAround(transform.TransformPoint(rotationPoint), Vector3.forward, -90);
        }

        if (Time.time - previousTime > (Input.GetKey(KeyCode.DownArrow) ? fallTime / 10 : fallTime))
        {
            transform.position += (Vector3.down);
            if (!ValidMove())
            {

                transform.position += (Vector3.up);
                AddToGrid();
                CheckForLines();
                this.enabled = false;
                FindObjectOfType<Spawner>().NewTetromino();
            }
            previousTime = Time.time;
        }
    }


    void CheckForLines()
    {
        for(int i = h-1; i>=0; i--)
        {
            if(HasLine(i))
            {
                DeleteLine(i);
                RowDown(i);
            }
        }


    }


    bool HasLine(int i)
    {
        for(int j =0; j<w; j++)
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
        for(int y =i; y<h; y++)
        {
            for(int j =0; j<w; j++)
            {
                if(grid[j, y] != null)
                {
                    grid[j, y - 1] = grid[j, y];
                    grid[j, y] = null;
                    grid[j, y - 1].transform.position += Vector3.down;
                }
            }
        }
    }

    void AddToGrid()
    {
        foreach (Transform child in transform)
        {
            int x = Mathf.RoundToInt(child.transform.position.x);
            int y = Mathf.RoundToInt(child.transform.position.y);
            grid[x, y] = child;
        }
    }

    private bool ValidMove()
    {
        foreach(Transform child in transform)
        {
            int x = Mathf.RoundToInt(child.transform.position.x);
            int y = Mathf.RoundToInt(child.transform.position.y);
            if (x < 0 || x >= w || y < 0 || y >= h) return false;
            if (grid[x, y] != null) return false;
        }
        return true;
    }
}
