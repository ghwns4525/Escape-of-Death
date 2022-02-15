using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnemysenser : MonoBehaviour
{
    SphereCollider sphareCollider;
    [SerializeField] float range;
    [SerializeField] Transform playerTr;
    private float moveSpeed = 5.0f;

    MonsterAI monsterAI;

    AudioSource[] audio;
    GameObject audioObject;

    // Start is called before the first frame update
    void Start()
    {
        audioObject = GameObject.Find("SoundManager");
        audio = audioObject.GetComponents<AudioSource>();
        monsterAI = FindObjectOfType<MonsterAI>();
        sphareCollider = GetComponent<SphereCollider>();
        sphareCollider.radius = range;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, playerTr.position + new Vector3(0, 1, 0), Time.deltaTime * moveSpeed);  
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "MONSTER")
        {
            if(!audio[2].isPlaying)
            {
                SoundManager.Instance.PlayerAudioClip(10, true, false);
            }            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "MONSTER")
        {
            SoundManager.Instance.StopAudioClip(true, false);
            //SoundManager.Instance.RestartAudioClip();
        }
    }
}
