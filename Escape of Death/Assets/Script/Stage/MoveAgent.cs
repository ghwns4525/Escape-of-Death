using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveAgent : MonoBehaviour
{
    public bool IsPatrolling
    {
        get { return isPatrolling; }
        set
        {
            isPatrolling = value;
            if (isPatrolling)
            {
                navMeshAgent.speed = patrolSpeed;
                damping = 6.0f;
                MoveWayPoint();
            }
        }
    }


    public Vector3 TraceTargetPos
    {
        get { return traceTargetPos; }
        set
        {
            traceTargetPos = value;
            navMeshAgent.speed = traceSpeed;
            damping = 14.0f;
            TraceTarget(traceTargetPos);
        }
    }
    public Vector3 MoveToReturnPoint
    {
        get { return returnPoint; }
        set
        {
            returnPoint = value;
            navMeshAgent.speed = patrolSpeed;
            damping = 6.0f;
            TraceTarget(returnPoint);
        }
    }

    public Transform TrapPoint 
    {
        get
        {
            return trapPoint;
        }
        set
        {
            if(monsterAI.EState != MonsterAI.eMonsterState.TRACE&& monsterAI.EState != MonsterAI.eMonsterState.ATTACK)
            {
                trapPoint = value;
                MoveTrapPoint();
            }          
        }
    }

    public bool IsTrapPoint { get => isTrapPoint; set => isTrapPoint = value; }

    public float GetNavMeshAgentSpeed()
    {
        return navMeshAgent.velocity.magnitude;
    }

    [Header("몬스터 이동속도,회전속도")]
    [SerializeField] private float patrolSpeed;
    [SerializeField] private float traceSpeed;
    [SerializeField] private float damping;

    [SerializeField] private List<Transform> wayPoints; //정찰할 포인트 리스트
    [SerializeField] private int nextPointIndex; // 다음 이동 포인트 인덱스
    private int randomPatrolPoint;
    private NavMeshAgent navMeshAgent; 
    private Transform monsterTr; // 해당 몬스터 위치
    [SerializeField] private string wayPointGroupName;

    [SerializeField] private bool isPatrolling; // 순찰중인지를 판단
    private Vector3 traceTargetPos; // 추적할 타겟 위치
    private Vector3 returnPoint; // 돌아갈 위치
    private bool isDestination = false;
    private MonsterAI monsterAI;

    [SerializeField] private Transform trapPoint;
    private bool isTrapPoint = false;

    // Start is called before the first frame update
    void Start()
    {
        monsterTr = GetComponent<Transform>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        monsterAI = GetComponent<MonsterAI>();
        navMeshAgent.autoBraking = false;
        navMeshAgent.updateRotation = false;
        navMeshAgent.speed = patrolSpeed;
        if (monsterAI.EType == MonsterAI.eAIType.TYPE_A)
        {
            var group = GameObject.Find(wayPointGroupName);
            if (group != null)
            {
                group.GetComponentsInChildren<Transform>(wayPoints);
                wayPoints.RemoveAt(0);
                nextPointIndex = 0;
            }
            MoveWayPoint();
            this.isPatrolling = true;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(monsterAI.EType == MonsterAI.eAIType.TYPE_A)
        {
            if (!navMeshAgent.isStopped)
            {
                RotateMonster();
            }

            if (!isPatrolling)
            {
                isTrapPoint = false;
                trapPoint = null;
                return;
            }
            if (navMeshAgent.velocity.sqrMagnitude >= 0.2f * 0.2f && navMeshAgent.remainingDistance <= 0.5f)
            {
                if (isTrapPoint)
                {
                    isTrapPoint = false;
                    StopMove();
                    Invoke("StartMove", 4.3f);                    
                    return;
                }

                if (navMeshAgent.isPathStale)
                {
                    return;
                }
                if (nextPointIndex >= wayPoints.Count-1)
                {
                    nextPointIndex=0;
                }
                else
                {
                    nextPointIndex++;
                }
                Debug.Log(nextPointIndex);

                MoveWayPoint();
            }
        }
        
    }
    public void RotateMonster()
    {
        Quaternion rot = Quaternion.LookRotation(navMeshAgent.desiredVelocity);
        monsterTr.rotation = Quaternion.Slerp(monsterTr.rotation, rot, Time.deltaTime * damping);
    }
    public void MoveWayPoint()
    {
        randomPatrolPoint = Random.Range(0, wayPoints.Count);
        navMeshAgent.destination = wayPoints[nextPointIndex].position;
        navMeshAgent.isStopped = false;
    }
    public void MoveTrapPoint()
    {
        isTrapPoint = true;
        navMeshAgent.destination = trapPoint.position;
        navMeshAgent.isStopped = false;

    }
    public void TraceTarget(Vector3 target)
    {
        if (navMeshAgent.isPathStale)
        {
            return;
        }
       
        navMeshAgent.destination = target;
        navMeshAgent.isStopped = false;
    }
    public void StopMove()
    {
        navMeshAgent.isStopped = true;
        navMeshAgent.velocity = Vector3.zero;
        if(monsterAI.EType == MonsterAI.eAIType.TYPE_A)
        {
            IsPatrolling = false;
            if(trapPoint != null)
            {
                trapPoint = null;
            }
        }
        
    }
    public void StartMove()
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.velocity = Vector3.zero;
        if (monsterAI.EType == MonsterAI.eAIType.TYPE_A)
        {
            IsPatrolling = true;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
       
        if(other.tag == "WayPoint")
        {
            if(randomPatrolPoint == nextPointIndex)
            {
                StopMove();
                Invoke("StartMove", 4.3f);
            }
           
        }
        
        
        return;
    }
  
}
