using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[Serializable]
public class NeuralNetwork
{
    public int hiddenSize, numHiddenLayers, inputSize, outputSize;
    public List<Neuron> inputLayer;
    public List<List<Neuron>> hiddenLayers;
    public List<Neuron> outputLayer;

    public NeuralNetwork(int inputSize, int hiddenSize, int outputSize, int numHiddenLayers)
    {
     
        inputLayer = new List<Neuron>();
        this.inputSize = inputSize;
        this.outputSize = outputSize;
        this.hiddenSize = hiddenSize;
        this.numHiddenLayers = numHiddenLayers;
        hiddenLayers = new List<List<Neuron>>();
        outputLayer = new List<Neuron>();
        
        for(var i = 0; i < this.inputSize; i++)
        {
            inputLayer.Add(new Neuron());
        }

        for(int i = 0; i< this.numHiddenLayers; i++)
        {
            hiddenLayers.Add(new List<Neuron>());
            for(int j = 0; j < this.hiddenSize; j++)
            {
                hiddenLayers[i].Add(new Neuron(i == 0 ? inputLayer : hiddenLayers[i - 1]));
            }
        }

        for(int i = 0; i < this.outputSize; i++)
        {
            outputLayer.Add(new Neuron(hiddenLayers[numHiddenLayers - 1]));
        }
        float[] inp = { 1.0f, 1.0f, 1.0f, 1.0f };
        //ForwardPropagate(inp);
    }



    public float[] ForwardPropagate(float[] input)
    {
        var i = 0;
        inputLayer.ForEach(a => a.Value = input[i++]);
        foreach(var layer in hiddenLayers)
        {
            layer.ForEach(a => a.Calculate());
        }
        outputLayer.ForEach(a => a.Calculate());
        return outputLayer.Select(a => a.Value).ToArray();
    }
}
