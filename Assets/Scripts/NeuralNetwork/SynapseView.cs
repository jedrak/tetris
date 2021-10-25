using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynapseView : MonoBehaviour
{
    public Synapse synapse;
    private LineRenderer lineRenderer;
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Color buff = synapse.Weight > 0 ? new Color(0, 1, 0, synapse.Weight): new Color(1, 0, 0, Mathf.Abs(synapse.Weight));

        lineRenderer.startColor = buff;
        lineRenderer.endColor = buff;
    }
}
