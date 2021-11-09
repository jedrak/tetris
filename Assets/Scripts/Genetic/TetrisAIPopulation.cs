using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
public class TetrisAIPopulation : MonoBehaviour, IPopulation<GameManager>
{
    public int PopulationCount = 100;
    public GameObject AIprefab;
    public List<GameManager> population;
    private int generationCounter = 0;
    [SerializeField] private float weight = 10, height = 20, padding = .5f;

    [SerializeField] private int w;
    [SerializeField] private TextMeshProUGUI info;

    public GameObject Breed(GameManager parent1, GameManager parent2)
    {
        GameObject ret = Instantiate(AIprefab);
        ret.transform.parent = transform;
        GameManager kidManager = ret.GetComponent<GameManager>();
        int i = 0, j = 0, k = 0;
        foreach(List<Neuron> layer in kidManager.nn.hiddenLayers)
        {
            j = 0;
            foreach(Neuron n in layer)
            {
                k = 0;
                foreach(var input in n.Inputs)
                {
                    //Debug.Log("i " + i + " j " + j + " k " + k);
                    input.Weight = Random.Range(0.0f, 1.0f) > .5f ? parent1.nn.hiddenLayers[i][j].Inputs[k].Weight : parent2.nn.hiddenLayers[i][j].Inputs[k].Weight;
                    k++;
                }
                j++;
            }
            i++;
        }
        k = 0; j = 0;
        foreach (Neuron n in kidManager.nn.outputLayer)
        {
            k = 0;
            foreach (var input in n.Inputs)
            {
                input.Weight = Random.Range(0.0f, 1.0f) > .5f ? parent1.nn.outputLayer[j].Inputs[k].Weight : parent2.nn.outputLayer[j].Inputs[k].Weight;
                k++;
            }
            j++;
        }

        return ret;
    }

    public void BreedNewPopulation()
    {
        List<GameManager> sorted = population.OrderByDescending(o => Fitness(o)).ToList();
        info.text = "Generation: " + (generationCounter + 1) + "				best player fitness: " + Fitness(sorted[0]);
        Debug.Log(Fitness(sorted[0]) + " " + Fitness(sorted.Last()));
        population.Clear();
        for(int i = 0; i < (sorted.Count/2); i++)
        {
            population.Add(Breed(sorted[i], sorted[i+1]).GetComponent<GameManager>());
            population.Last().transform.position = Vector3.right * (weight + padding) * ((i*2) % w) + Vector3.up * (height + padding) * ((i*2) / w);
            population.Add(Breed(sorted[i+1], sorted[i]).GetComponent<GameManager>());
            population.Last().transform.position = Vector3.right * (weight + padding) * ((i * 2 + 1) % w) + Vector3.up * (height + padding) * ((i * 2 + 1) / w);

        }

        foreach (var it in sorted)
        {
            Destroy(it.gameObject);
        }

        foreach(var gm in population)
        {
            if (Random.Range(0, 1.0f) < .0005f) Mutate(gm);
        }
        generationCounter++;
    }

    public float Fitness(GameManager dna)
    {
        //Debug.Log(dna.gameObject.name + " " + (float)dna.GetHeights().Sum() / (float)w);
        return (float) (dna.moveCounter - dna.GridH() - dna.GetHeights().Sum()/w /*+ (80.0f*dna.linesDeleted)*/ - .5f*dna.GetHoles() - 2*dna.GetBumpines() + dna.moveScore);
    }

    public void Mutate(GameManager Kid)
    {
        ///TODO implement mutation
        int i = 0, j = 0, k = 0;
        foreach (List<Neuron> layer in Kid.nn.hiddenLayers)
        {
            j = 0;
            foreach (Neuron n in layer)
            {
                k = 0;
                foreach (var input in n.Inputs)
                {
                    //Debug.Log("i " + i + " j " + j + " k " + k);
                    input.Weight = Random.Range(0.0f, 1.0f) > .5f ? Kid.nn.hiddenLayers[i][j].Inputs[k].Weight : Random.Range(-1, 1);
                    k++;
                }
                j++;
            }
            i++;
        }
        k = 0; j = 0;
        foreach (Neuron n in Kid.nn.outputLayer)
        {
            k = 0;
            foreach (var input in n.Inputs)
            {
                input.Weight = Random.Range(0.0f, 1.0f) > .5f ? Kid.nn.outputLayer[j].Inputs[k].Weight : Random.Range(-1, 1);
                k++;
            }
            j++;
        }
    }

    public void PopulationInit()
    {
        for(int i = 0; i < PopulationCount; i++)
        {
            GameObject go = Instantiate(AIprefab);
            go.transform.parent = transform;
            go.transform.position = Vector3.right * (weight + padding) * (i%w) + Vector3.up * (height + padding) * (i/w);
            population.Add(go.GetComponent<GameManager>());
        }
    }


    private void Awake()
    {
        PopulationInit();
    }

    void Update()
    {
        if (population.All(gm => gm.over))
        {
            BreedNewPopulation();
        }

        foreach(var p in population)
        {
            p.GetComponentInChildren<TextMeshProUGUI>().text = "PRAMETERS:\n" +
                "Lines: " + p.linesDeleted + "\n" +
                "H: " + p.GridH() + "\n" +
                "Avg. H: " + p.GetHeights().Sum()/10f + "\n" +
                "Moves: " + p.moveCounter + "\n" +
                "Fitness: " + Fitness(p) + "\n" +
                "Bumpiness: " + p.GetBumpines() + "\n" +
                "Holes: " + p.GetHoles() + "\n";
        }
    }
}
