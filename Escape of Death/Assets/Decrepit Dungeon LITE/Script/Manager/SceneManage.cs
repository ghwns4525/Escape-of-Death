using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LoadingState
{
    Load,   // �Ϲ��� �� �ε�
    ReLoad, // �ٽ��ϱ� �ε�
}
public class SceneManage : MonoBehaviour
{
    [SerializeField] private int sceneNum;
    public LoadingState state;
    public int SceneNum { get => sceneNum; set => sceneNum = value; }

    // Start is called before the first frame update
    void Start()
    {
        GameObject.DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
