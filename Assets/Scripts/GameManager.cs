using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<Block> Blocks;
    private float update = 1.0f;
    private float timer = 0.0f;

    private void Start()
    {
        
    }

    private void Update()
    {
        if(timer > update)
        {
            timer = 0.0f;
            foreach(Block b in Blocks)
            {
                //b.BlockTranslate(Vector2.down * .5f);
            }
        }
        timer += Time.deltaTime;
    }
}
