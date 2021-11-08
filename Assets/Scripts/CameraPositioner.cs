using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class CameraPositioner : MonoBehaviour
{
    public TetrisAIPopulation population;
    public Vector3 offset = 2 * Vector3.forward;

    void Update()
    {
        transform.position = population.population.OrderByDescending(o => population.Fitness(o)).ToList()[0].transform.position - offset;
    }
}
