using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingManager : MonoBehaviour
{
    [SerializeField] bool isBackGround = true;
    [SerializeField] bool isSoundEffect = true;
    private GameObject soundManager;
    private AudioSource[] audioSources;

    public bool IsBackGround { get => isBackGround; set => isBackGround = value; }
    public bool IsSoundEffect { get => isSoundEffect; set => isSoundEffect = value; }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        soundManager = GameObject.Find("SoundManager");
        audioSources = soundManager.GetComponents<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isSoundEffect)
        {
            audioSources[0].volume = 1;
        }
        else
        {
            audioSources[0].volume = 0;
        }


        if(isBackGround)
        {
            audioSources[1].volume = 1;
            audioSources[2].volume = 1;
        }
        else
        {
            audioSources[1].volume = 0;
            audioSources[2].volume = 0;
        }
        Debug.Log(isBackGround);
        Debug.Log(isSoundEffect);
    }

    #region == BtnEvts ==  

    #endregion
}
