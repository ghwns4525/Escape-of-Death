using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeScene : MonoBehaviour
{
    [SerializeField] private Image fadeScene;
    float duration = 10;
    float smoothness = 0.02f;
    
    private void Awake()
    {
        StartCoroutine(SetColor());
    }
    
    IEnumerator SetColor()
    {
        float progress = 0;
        float increment = smoothness / duration;
        while (progress < 1)
        {
            fadeScene.color = Color.Lerp(fadeScene.color, Color.clear, progress);
            progress += increment;
            yield return new WaitForSeconds(smoothness);

        }
        yield return null;
    }

}
