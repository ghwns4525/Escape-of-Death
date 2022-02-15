using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KeySpawn : MonoBehaviour
{
    public List<BoxCollider> spawnPoints = new List<BoxCollider>();
    [SerializeField] private GameObject keyPrefab;
    public BoxCollider area;
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            spawnPoints.Add(transform.GetChild(i).GetComponent<BoxCollider>());
        }
        area = spawnPoints[Random.Range(0, spawnPoints.Count)];
        
        spawn();
    }

    void spawn()
    {
        Vector3 size = area.size;
        float rndRangeX = Random.Range(-(size.x/2), size.x/2);
        float rndRangeZ = Random.Range(-(size.z / 2), size.z/2);
        Vector3 rndRange = new Vector3(area.transform.position.x + rndRangeX, 0.9f, area.transform.position.z + rndRangeZ);
        GameObject instance = Instantiate(keyPrefab, rndRange, Quaternion.identity);
    }
}

