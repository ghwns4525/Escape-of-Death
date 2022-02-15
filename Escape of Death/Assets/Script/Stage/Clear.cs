using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Clear : MonoBehaviour
{
    SceneManage sceneManage;
    // Start is called before the first frame update
    void Start()
    {
        sceneManage = FindObjectOfType<SceneManage>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "PLAYER")
        {
            // Clear 사운드를 늦게 추가해서 맨 뒷번호
            SoundManager.Instance.PlayerAudioClip(15, false, false);
            if(SceneManager.GetActiveScene().name == "Stage05_Out")
            {
                SoundManager.Instance.StopAudioClip(true, false);
                SceneManager.LoadScene("ClearScene");
            }
            else
            {
                SoundManager.Instance.StopAudioClip(true, false);
                sceneManage.state = LoadingState.Load;
                SceneManager.LoadScene("LoadingScene");
                
            }
            
        }        
    }
}
