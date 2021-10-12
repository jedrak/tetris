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
        GameObject go = Instantiate(Tetrominoes[Random.Range(0, Tetrominoes.Length)], transform.position, Quaternion.identity);
        go.transform.parent = transform;
        if (!_manager.ValidMove(go.transform)) _manager.GameOver();
    }
}
