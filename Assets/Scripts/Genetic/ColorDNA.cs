using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorDNA : DNA
{

    public Color color;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().color = color;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
