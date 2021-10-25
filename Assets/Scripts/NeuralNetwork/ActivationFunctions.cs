using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ActivationFunction
{
    public abstract float Output(float x);
    public abstract float Diverative(float x);
}


public class Sigmoid : ActivationFunction
{
    public float Diverative(float x)
    {
        throw new System.NotImplementedException();
    }

    public float Output(float x)
    {
        return x < -45.0f ? 0.0f : x > 45.0f ? 1.0f : 1.0f / (1.0f + Mathf.Exp(-x));
    }
}