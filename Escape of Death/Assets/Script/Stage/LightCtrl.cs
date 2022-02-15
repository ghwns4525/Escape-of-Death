using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightCtrl : MonoBehaviour
{
    private Light light;
    private bool isLight = false;
    [SerializeField] private float lightRange;

    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light>();
    }
        

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            isLight = !isLight;
            SoundManager.Instance.PlayerAudioClip(5, false, false);
        }
        if(isLight)
        {
            light.range = lightRange;
        }
        else
        {
            light.range = 0f;
        }
    }
}
