using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodScreen : MonoBehaviour
{
    private Image image;
    private Charactermove player;    
    private Color color;

    private bool isActive;
    void Start()
    {
        image = GetComponent<Image>();
        player = FindObjectOfType<Charactermove>();
    }

    // Update is called once per frame
    void Update()
    {
        if(player.State == Charactermove.eState.DIE)
        {            
            color = image.color;
            if(!isActive)
            {
                color.a += 1 * Time.deltaTime;
                image.color = color;
                if(color.a > 80)
                {
                    isActive = true;
                }                               
            }           
            if(isActive)
            {
                color.a -= 1 * Time.deltaTime;
                image.color = color;
            }
        }
    }
}
