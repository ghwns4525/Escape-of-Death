using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    SceneManage sceneManage;

    // Start is called before the first frame update
    void Start()
    {
        sceneManage = FindObjectOfType<SceneManage>();
        if(SceneManager.GetActiveScene().name != "DeathScene")
        {
           sceneManage.SceneNum = SceneManager.GetActiveScene().buildIndex;
        }
        

        if(SceneManager.GetActiveScene().name == "Main_UI")
        {
            SoundManager.Instance.PlayerAudioClip(1, true, true);
        }
        else if(SceneManager.GetActiveScene().name == "DeathScene")
        {
            SoundManager.Instance.StopAudioClip(true, false);
            SoundManager.Instance.PlayerAudioClip(3, false, false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
