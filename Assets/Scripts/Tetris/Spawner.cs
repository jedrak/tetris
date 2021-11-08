using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] Tetrominoes;
    GameManager _manager;
    // Start is called before the first frame update
    void Start()
    {
        _manager = GetComponent<GameManager>();
        NewTetromino();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewTetromino()
    {
        _manager.currentBlockType = Random.Range(0, Tetrominoes.Length);
        GameObject go = Instantiate(Tetrominoes[_manager.currentBlockType], transform.position, Quaternion.Euler(0, 0, 90*Random.Range(0, 3)));
        go.transform.parent = transform;
        _manager.current = go.GetComponent<Block>();
        if (!_manager.ValidMove(go.transform))
        {
            _manager.GameOver();
            Destroy(go);
        }
        if (go.GetComponent<BlockInputListener>() != null) go.GetComponent<BlockInputListener>().nn = _manager.nn;

    }
}
