using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NeuronView : MonoBehaviour
{
    [SerializeField] private GameObject LineView;

    [SerializeField] public Neuron neuron { get; set; }
    [SerializeField] private List<SynapseView> inputRenderers;
    private TextMeshProUGUI text;
    
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        //neuron = new Neuron();
        foreach (var s in neuron.Inputs)
        {
            var go = Instantiate(LineView, transform);
            go.GetComponent<SynapseView>().synapse = s;
            go.transform.position = transform.position;
            go.GetComponent<LineRenderer>().SetPosition(0, neuron.view.transform.position);
            go.GetComponent<LineRenderer>().SetPosition(1, s.InputNeuron.view.transform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        text.text = neuron.Value.ToString("0.00");
    }
}
