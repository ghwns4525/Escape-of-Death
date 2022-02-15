using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LoadingGauge : MonoBehaviour
{
    private Image gauge;
    private float timer = 0.0f;
    private int sceneNum = 0;
    SceneManage sceneManage;
    LoadingState state;

    // Start is called before the first frame update
    void Start()
    {
        sceneManage = FindObjectOfType<SceneManage>();
        gauge = GetComponent<Image>();
        sceneNum = sceneManage.SceneNum;
        state = sceneManage.state;
    }

    // Update is called once per frame
    void Update()
    {
        Loading();
        switch(state)
        {
            case LoadingState.Load:
                SceneChange();
                break;
            case LoadingState.ReLoad:
                ReLoad();
                break;
        }        
    }

    void Loading()
    {
        timer += Time.deltaTime;
        gauge.fillAmount = Mathf.Lerp(gauge.fillAmount/100, 1, timer);
    }

    void SceneChange()
    {
        if(gauge.fillAmount >= 1.0f)
        {
            SceneManager.LoadScene(sceneNum + 1);
        }        
    }

    void ReLoad()
    {
        if (gauge.fillAmount >= 1.0f)
        {
            SceneManager.LoadScene(sceneNum);
            sceneManage.state = LoadingState.Load;
        }
    }
}
