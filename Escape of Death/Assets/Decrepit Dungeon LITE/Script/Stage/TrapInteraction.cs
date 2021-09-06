using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapInteraction : MonoBehaviour
{
    [SerializeField] private MoveAgent moveAgent;
    [SerializeField] private string moveAgentName;

    private void Awake()
    {
        if(moveAgentName != null)
        {
            moveAgent = GameObject.Find(moveAgentName).GetComponent<MoveAgent>();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "PLAYER")
        {
            if (moveAgent != null)
            {
                if(moveAgent.TrapPoint == null)
                {
                    moveAgent.TrapPoint = this.transform;
                }                
            }        
        }
        
    }
}
