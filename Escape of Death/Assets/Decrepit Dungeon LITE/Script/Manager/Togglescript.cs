using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Togglescript : MonoBehaviour {

    Toggle toggle;
    SettingManager settingManager;

    private void Start()
    {
        toggle = GetComponent<Toggle>();
        settingManager = FindObjectOfType<SettingManager>();
        if (this.gameObject.name == "BGM_Toggle")
        {
            Bgm_Toggle();
        }
        else if (this.gameObject.name == "Effect_Toggle")
        {
            Effect_Toggle();
        }
    }

    void Bgm_Toggle()
    {
        if(settingManager.IsBackGround)
        {
            toggle.isOn = true;
        }
        else
        {
            toggle.isOn = false;
        }
    }

    void Effect_Toggle()
    {
        if (settingManager.IsSoundEffect)
        {
            toggle.isOn = true;
        }
        else
        {
            toggle.isOn = false;
        }
    }
}
