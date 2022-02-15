using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioClip[] audioclip;
    private void Start()
    {
        GameObject.DontDestroyOnLoad(this);
        SoundManager.Instance.CreateDefaultAudioSource();
        for (int i = 0; i < audioclip.Length; i++)
        {
            SoundManager.Instance.Regist(i+1, audioclip[i]);
        }
        SettingStart();
    }

    void SettingStart()
    {
        GameObject oSettingManager = GameObject.Find("SettingManager");
        {
            oSettingManager = new GameObject("SettingManager");
            Debug.Assert(oSettingManager != null, "Can not create new SoundManager GameeObject");
        }
        GameObject.DontDestroyOnLoad(oSettingManager);

        oSettingManager.AddComponent<SettingManager>();
    }

}
