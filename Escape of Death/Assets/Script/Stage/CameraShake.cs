using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class CameraShake : MonoBehaviour
{
    Vector3 originPos;
    private Charactermove charactermove;
    // Start is called before the first frame update
    void Start()
    {
        originPos = transform.localPosition;
        charactermove = FindObjectOfType<Charactermove>();
    }

    public IEnumerator Co_Shake()
    {
       
        while (true)
        {
            if (charactermove != null)
            {
                if (charactermove.IsRun)
                {
                    Debug.Log("코루틴 호출");
                    transform.DOLocalMove(new Vector3(0, 1.92f, 0.1f), 0.3f);
                    yield return new WaitForSeconds(0.3f);
                    transform.DOLocalMove(new Vector3(0, 1.88f, 0.1f), 0.3f);
                    yield return new WaitForSeconds(0.3f);
                }
                else
                {
                    transform.DOLocalMove(new Vector3(0, 1.9f, 0.1f), 0.3f);
                    yield return new WaitForSeconds(0.3f);
                }
            }
            yield return new WaitForSeconds(0.1f);
        }      
    }
   
    // Update is called once per frame
    void Update()
    {
        
    }
}
