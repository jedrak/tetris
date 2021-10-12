using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Population : MonoBehaviour
{
    public List<GameObject> population;
    public GameObject prefab;
    public int populationSize = 100;
    public Color desirable = Color.red;
    public int generation = 0;
    public TextMeshProUGUI generationText;
    private float mutateRate = 0.005f;
    private float timer = .0f;
    private float Fitness(ColorDNA dna)
    {
        float distance = (desirable.r - dna.color.r) * (desirable.r - dna.color.r) 
            + (desirable.g - dna.color.g) * (desirable.g - dna.color.g) 
            + (desirable.b - dna.color.b) * (desirable.b - dna.color.b);
        return distance;
    }

    void PopulationInit()
    {
        for(int i = 0; i < populationSize; i++)
        {
            Vector3 p = new Vector3(Random.Range(-10, 10), Random.Range(-4.5f, 4.5f), -1);
            GameObject go = Instantiate(prefab, p, Quaternion.identity);
            go.GetComponent<ColorDNA>().color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
            go.transform.parent = transform;
            go.name += i;
            //Debug.Log(go.GetComponent<ColorDNA>().color + " " + Fitness(go.GetComponent<ColorDNA>()));
            population.Add(go);
        }
    }

    GameObject Breed(Color parent1, Color parent2)
    {
        Vector3 p = new Vector3(Random.Range(-10, 10), Random.Range(-4.5f, 4.5f), -1);
        GameObject go = Instantiate(prefab, p, Quaternion.identity);
        Color c = new Color(Random.Range(0, 10)<5 ? parent1.r : parent2.r, Random.Range(0, 10) < 5 ? parent1.g : parent2.g, Random.Range(0, 10) < 5 ? parent1.b : parent2.b);
        go.GetComponent<ColorDNA>().color = c;
        go.transform.parent = transform;
        return go;
    }

    void Mutate(GameObject Kid)
    {
        Kid.GetComponent<ColorDNA>().color.r = Random.Range(0.0f, 1.0f) < mutateRate ? Random.Range(0.0f, 1.0f) : Kid.GetComponent<ColorDNA>().color.r;
        Kid.GetComponent<ColorDNA>().color.g = Random.Range(0.0f, 1.0f) < mutateRate ? Random.Range(0.0f, 1.0f) : Kid.GetComponent<ColorDNA>().color.g;
        Kid.GetComponent<ColorDNA>().color.b = Random.Range(0.0f, 1.0f) < mutateRate ? Random.Range(0.0f, 1.0f) : Kid.GetComponent<ColorDNA>().color.b;
    }

    void BreedNewPopulation()
    {
        List<GameObject> sorted = population.OrderByDescending(o => Fitness(o.GetComponent<ColorDNA>())).Reverse().ToList();

        population.Clear();
        for(int i = 0; i < (sorted.Count/2)-1; i++)
        {
            population.Add(Breed(sorted[i].GetComponent<ColorDNA>().color, sorted[i + 1].GetComponent<ColorDNA>().color));
            population.Add(Breed(sorted[i + 1].GetComponent<ColorDNA>().color, sorted[i].GetComponent<ColorDNA>().color));
        }
        foreach(GameObject go in population)
        {
            Mutate(go);
        }
        for (int i = 0; i < sorted.Count; i++)
        {
            Destroy(sorted[i]);
        }
        generation++;
    }

    private void Awake()
    {
        PopulationInit();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > 2)
        {
            BreedNewPopulation();
            timer = 0;
            generationText.text = "Generation " + generation;
        }

    }
}
