using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Block : MonoBehaviour
{
    private float previousTime;
    public float fallTime = .8f;
    public static int h = 20;
    public static int w = 10;
    public Vector3 rotationPoint;
    public GameManager _GameManager;
    void Start()
    {
        _GameManager = GetComponentInParent<GameManager>();
    }

    public float GetPlacementH()
    {
        float f = 0f;
        foreach(var t in transform)
        {
            int h = Mathf.RoundToInt(transform.position.y - _GameManager.corner.position.y);
            f += h;
        }
        return f/4.0f;
    }

    public float GetPlacementScore(Transform [,] grid)
    {
        float buff = 0.0f;
        Transform[] children = transform.GetComponentsInChildren<Transform>();
        foreach(Transform child in transform)
        {
            for(int i = 0; i < GameManager.w; i++)
            {
                for(int j = 0; j < GameManager.h; j++)
                {
                    if(grid[i,j] == child)
                    {
                        if (i == 0) buff++;
                        if (i == GameManager.w - 1) buff++;
                        if (j == 0) buff++;
                        if (j == GameManager.h - 1) buff++;


                        if (i < GameManager.w - 1 && grid[i + 1, j] != null && !children.Contains(grid[i + 1, j])) buff++;
                        if (i > 0 && grid[i - 1, j] != null && !children.Contains(grid[i - 1, j])) buff++;
                        if (j < GameManager.h - 1 && grid[i, j + 1] != null && !children.Contains(grid[i, j + 1])) buff++;
                        if (j > 0 && grid[i, j - 1] != null && !children.Contains(grid[i, j - 1])) buff++;

                    }
                }
            }
        }
        //Debug.Log(buff);
        return buff;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_GameManager.over)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                transform.position += (Vector3.left);
                if (!_GameManager.ValidMove(transform)) transform.position += (Vector3.right);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                transform.position += (Vector3.right);
                if (!_GameManager.ValidMove(transform)) transform.position += (Vector3.left);
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                transform.RotateAround(transform.TransformPoint(rotationPoint), Vector3.forward, 90);
                if (!_GameManager.ValidMove(transform)) transform.RotateAround(transform.TransformPoint(rotationPoint), Vector3.forward, -90);
            }

            if (Time.time - previousTime > (Input.GetKey(KeyCode.DownArrow) ? fallTime / 10 : fallTime))
            {

                transform.position += (Vector3.down);
                if (!_GameManager.ValidMove(transform))
                {

                    transform.position += (Vector3.up);
                    _GameManager.AddToGrid(transform);
                    GetPlacementScore(_GameManager.grid);

                    _GameManager.CheckForLines();

                    this.enabled = false;
                    _GameManager.GetComponent<Spawner>().NewTetromino();
                }
                previousTime = Time.time;
            }
        }

    }



}
