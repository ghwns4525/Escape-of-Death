using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonSceneManager : MonoBehaviour
{
    SceneManage sceneManage;
    SettingManager settingManager;
    Charactermove charactermove;
    // Start is called before the first frame update
    void Start()
    {
        sceneManage = FindObjectOfType<SceneManage>();
        charactermove = FindObjectOfType<Charactermove>();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        settingManager = FindObjectOfType<SettingManager>();
    }

    #region == BtnEvts ==  
    public void BtnEvt_Start()
    {        
        SoundManager.Instance.PlayerAudioClip(2, true, true);
        SceneManager.LoadScene("LoadingScene");
    }

    public void BtnEvt_Setting()
    {
        SoundManager.Instance.PlayerAudioClip(4, false, false);
        SceneManager.LoadScene("Setting_UI");
    }

    public void BtnEvt_Exit()
    {
        SoundManager.Instance.PlayerAudioClip(4, false, false);
        Application.Quit();
        
    }

    public void BtnEvt_Main()
    {
        SoundManager.Instance.PlayerAudioClip(4, false, false);
        SceneManager.LoadScene("Main_UI");
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }

    public void BtnEvt_ReTry()
    {
        SoundManager.Instance.PlayerAudioClip(4, false, false);
        sceneManage.state = LoadingState.ReLoad;
        SceneManager.LoadScene("LoadingScene");
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }

    public void BtnEvt_BackGround()
    {
        SoundManager.Instance.PlayerAudioClip(4, false, false);
        settingManager.IsBackGround = !settingManager.IsBackGround;
    }

    public void BtnEvt_SoundEffect()
    {
        SoundManager.Instance.PlayerAudioClip(4, false, false);
        settingManager.IsSoundEffect = !settingManager.IsSoundEffect;
    }

    public void BtnEvt_OptionExit()
    {
        charactermove.OptionPopup();
    }

    public void BtnEvt_1Stage()
    {
        SoundManager.Instance.PlayerAudioClip(2, true, true);
        SceneManager.LoadScene("Stage01_Prison");
    }
    public void BtnEvt_2Stage()
    {
        SoundManager.Instance.PlayerAudioClip(2, true, true);
        SceneManager.LoadScene("Stage02_Restaurant");
    }
    public void BtnEvt_3Stage()
    {
        SoundManager.Instance.PlayerAudioClip(2, true, true);
        SceneManager.LoadScene("Stage03_mine");
    }
    public void BtnEvt_4Stage()
    {
        SoundManager.Instance.PlayerAudioClip(2, true, true);
        SceneManager.LoadScene("Stage04_Wayout");
    }
    public void BtnEvt_5Stage()
    {
        SoundManager.Instance.PlayerAudioClip(2, true, true);
        SceneManager.LoadScene("Stage05_Out");
    }
    #endregion
}
