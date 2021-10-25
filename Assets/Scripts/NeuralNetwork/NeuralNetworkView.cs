using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuralNetworkView : MonoBehaviour
{


    [SerializeField] private GameObject layerView;
    [SerializeField] private GameObject neuronView;

    [SerializeField] private NeuralNetwork nn;
    [SerializeField] [Range(1.0f, 100.0f)] private float offsetDelta;
    // Start is called before the first frame update
    void Awake()
    {
       

        nn = new NeuralNetwork(4, 5, 3, 5);
        Vector3 offset = Vector3.up * offsetDelta * (nn.inputSize-1) / 2.0f;
        var inputLayerView = Instantiate(layerView, transform);
        inputLayerView.name = "input";
        foreach(Neuron n in nn.inputLayer)
        {
            var go = Instantiate(neuronView, inputLayerView.transform);
            go.GetComponent<NeuronView>().neuron = n;
            n.view = go.GetComponent<NeuronView>();
            go.transform.position += offset;
            offset += Vector3.down * offsetDelta;

        }

        offset = Vector3.right * offsetDelta + Vector3.up * offsetDelta * (nn.hiddenSize - 1) / 2.0f;
        foreach(List<Neuron> Layer in nn.hiddenLayers)
        {
            var layerGo = Instantiate(layerView, transform);
            foreach(Neuron n in Layer)
            {
                var go = Instantiate(neuronView, layerGo.transform);
                go.GetComponent<NeuronView>().neuron = n;
                n.view = go.GetComponent<NeuronView>();
                go.transform.position += offset;
                offset += Vector3.down * offsetDelta;
            }
            offset += Vector3.right * offsetDelta;
            offset.y =  offsetDelta * (nn.hiddenSize - 1) / 2.0f;
        }

        var outputLayerView = Instantiate(layerView, transform);
        outputLayerView.name = "output";
        offset.y = offsetDelta * (nn.outputSize - 1) / 2.0f;
        foreach (Neuron n in nn.outputLayer)
        {
            var go = Instantiate(neuronView, outputLayerView.transform);
            go.GetComponent<NeuronView>().neuron = n;
            n.view = go.GetComponent<NeuronView>();
            go.transform.position += offset;
            offset += Vector3.down * offsetDelta;

        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
