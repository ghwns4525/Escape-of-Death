using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolArea : MonoBehaviour
{
    [SerializeField] private MonsterAI monsterAI;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "PLAYER")
        {
            monsterAI.IsFindPlayer = true;
           
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "PLAYER")
        {
            if (monsterAI.EType == MonsterAI.eAIType.TYPE_B)
            {
                monsterAI.IsFindPlayer = false;
                monsterAI.IsMissPlayer = true;
                monsterAI.EState = MonsterAI.eMonsterState.MISSPLAYER;
            }
            else
            {
                if (monsterAI.EState == MonsterAI.eMonsterState.TRACE || monsterAI.EState == MonsterAI.eMonsterState.ATTACK)
                {
                    monsterAI.IsReturnPatrol = false;
                    monsterAI.IsFindPlayer = false;
                    monsterAI.IsMissPlayer = true;
                    monsterAI.EState = MonsterAI.eMonsterState.MISSPLAYER;
                }
                else
                {
                    monsterAI.IsFindPlayer = false;
                }
            }
        }
    }
}
