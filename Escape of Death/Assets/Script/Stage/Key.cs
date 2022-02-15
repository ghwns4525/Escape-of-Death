using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{ 
    private bool isLooking = false;

    public bool IsLooking { get => isLooking; set => isLooking = value; }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isLooking)
        {
            // �ݶ��̴� �׵θ� ����
            this.GetComponent<Renderer>().material.color = Color.yellow;
        }
        else
        {
            this.GetComponent<Renderer>().material.color = Color.white;
        }
    }
}
