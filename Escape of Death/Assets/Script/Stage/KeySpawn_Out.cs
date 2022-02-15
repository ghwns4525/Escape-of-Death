using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeySpawn_Out : MonoBehaviour
{
    private Transform[] spawnPoints;
    [SerializeField] private GameObject keyPrefab;
    // Start is called before the first frame update
    void Start()
    {
        spawnPoints = GetComponentsInChildren<Transform>();
        Spawn();
    }

    private void Spawn()
    {
        int rand = Random.Range(0, spawnPoints.Length);
        Instantiate(keyPrefab, spawnPoints[rand].position, Quaternion.identity);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
