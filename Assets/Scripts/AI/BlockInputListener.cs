using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class BlockInputListener : Block
{
    public NeuralNetwork nn;
    private void Awake()
    {
        //nn = new NeuralNetwork(4, 5, 4, 5);
    }
    void Update()
    {
        float[] input = _GameManager.GetInput();
        var output = nn.ForwardPropagate(input);
        
        int move = Array.IndexOf(output, output.Max());

        //Debug.Log(string.Join(" ", output) + " " + move, this);
        if (!_GameManager.over)
        {
            if (move == 0)
            {
                transform.position += (Vector3.left);
                if (!_GameManager.ValidMove(transform)) transform.position += (Vector3.right);
            }
            else if (move == 1)
            {
                transform.position += (Vector3.right);
                if (!_GameManager.ValidMove(transform)) transform.position += (Vector3.left);
            }
            else if (move == 2)
            {
                transform.RotateAround(transform.TransformPoint(rotationPoint), Vector3.forward, 90);
                if (!_GameManager.ValidMove(transform)) transform.RotateAround(transform.TransformPoint(rotationPoint), Vector3.forward, -90);
            }
            
            if (true)
            {

                transform.position += (Vector3.down);
                if (!_GameManager.ValidMove(transform))
                {
                    _GameManager.moveCounter += GameManager.h - (int) GetPlacementH();
                    transform.position += (Vector3.up);
                    _GameManager.AddToGrid(transform);
                    _GameManager.moveScore += (int) GetPlacementScore(_GameManager.grid);
                    _GameManager.CheckForLines();
                    this.enabled = false;
                    
                    _GameManager.GetComponent<Spawner>().NewTetromino();
                }
            }
        }
    }
}
