using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPopulation<T>
{
    public float Fitness(T dna);
    public void PopulationInit();
    public GameObject Breed(T parent1, T parent2);
    public void Mutate(T Kid);
    public void BreedNewPopulation();

}
