using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    private float previousTime;
    public float fallTime = .8f;
    public static int h = 20;
    public static int w = 10;
    public Vector3 rotationPoint;
    private GameManager _GameManager;
    void Start()
    {
        _GameManager = GetComponentInParent<GameManager>();
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
                    _GameManager.CheckForLines();
                    this.enabled = false;
                    _GameManager.GetComponent<Spawner>().NewTetromino();
                }
                previousTime = Time.time;
            }
        }

    }



}
