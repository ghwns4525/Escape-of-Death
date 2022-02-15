using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Effect_Check : MonoBehaviour
{
    SettingManager settingManager;
    // Start is called before the first frame update
    void Start()
    {
        settingManager = FindObjectOfType<SettingManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(settingManager.IsSoundEffect)
        {
            this.gameObject.GetComponent<Image>().color = new Color(255,255,255,255);
        }
        else if(!settingManager.IsSoundEffect)
        {
            this.gameObject.GetComponent<Image>().color = new Color(255, 255, 255, 0);
        }
    }
}
