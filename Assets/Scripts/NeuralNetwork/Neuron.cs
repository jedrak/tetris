using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class Neuron
{
    public List<Synapse> Inputs;
    public List<Synapse> Outputs;
    //public float Bias;
    public float Value;
    public ActivationFunction activationFunction;
    public NeuronView view;


    public Neuron()
    {
        Inputs = new List<Synapse>();
        Outputs = new List<Synapse>();
        //Bias = (float) new Random((int)UnityEngine.Time.time).NextDouble();
        activationFunction = new Sigmoid();
    }

    public Neuron(IEnumerable<Neuron> inputNeurons) : this()
    {
        foreach(Neuron n in inputNeurons)
        {
            var s = new Synapse(n, this);
            this.Inputs.Add(s);
            n.Outputs.Add(s);
        }
    }

    public float Calculate()
    {
        return Value = activationFunction.Output(Inputs.Sum(s => s.Weight * s.InputNeuron.Value));
    }
    
}


public class Synapse
{
    public Neuron InputNeuron;
    public Neuron OutputNeuron;
    public float Weight;

    public Synapse(Neuron n, Neuron neuron)
    {
        InputNeuron = n;
        OutputNeuron = neuron;
        Weight = UnityEngine.Random.Range(-1.0f, 1.0f);
    }
}
