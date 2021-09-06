using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MonsterAI : MonoBehaviour
{
    public enum eAIType
    {
        TYPE_A,
        TYPE_B
    }
    public enum eMonsterState
    {
        IDLE,
        PATROL,
        TRACE,
        ATTACK,
        DIE,
        MISSPLAYER

    }
    public eMonsterState EState { get => eState; set => eState = value; }
    public eAIType EType { get => eType; set => eType = value; }
    public bool IsFindPlayer { get => isFindPlayer; set => isFindPlayer = value; }
    public bool IsMissPlayer { get => isMissPlayer; set => isMissPlayer = value; }
    public bool IsReturnPatrol { get => isReturnPatrol; set => isReturnPatrol = value; }

    [SerializeField] private eMonsterState eState = eMonsterState.IDLE;
    [SerializeField] private eAIType eType;

    [Header("플레이어 상실 시 몬스터가 돌아갈 위치 (B타입 전용)")]
    [SerializeField] private Transform returnPoint;
    private Transform playerTr;
    private Transform monsterTr;
    private Transform target; // 플레이어가 몬스터 감지범위 안에 들어오면 할당. 벗어나면 null

    [Header("플레이어와 몬스터 사이의 벽 감지")]
    [SerializeField] private bool isBlindWall = true;
    private bool isReturn = false;
    private bool isMissPlayer = false;
    [SerializeField] private bool isFindPlayer = false;
    private bool isReturnPatrol = true;

    [Header("AI 공격 사거리")]
    [SerializeField] private float attackDist = 9.0f;
    [Header("AI 플레이어 감지 거리 & 플레이어 상실 범위(A타입 전용)")]
    [SerializeField] private float traceDist = 16.0f;
    [SerializeField] private float missPlayerDist = 20.0f;

    #region == AnimatiorParameters ==
    private readonly int hashPatrolling = Animator.StringToHash("IsPatrolling");
    private readonly int hashTracing = Animator.StringToHash("IsTracing");
    private readonly int hashAttack = Animator.StringToHash("Attack");
    private readonly int hashPatrollAround = Animator.StringToHash("IsPatrollAround");
    private readonly int hashFindPlayer = Animator.StringToHash("FindPlayer");
    private readonly int hashReturn = Animator.StringToHash("IsReturn");
    private readonly int hashMissPlayer = Animator.StringToHash("MissPlayer");
    #endregion


    private MoveAgent moveAgent;
    private Animator animator;
    private RaycastHit hit;

    //코루틴 변수들
    private IEnumerator co_checkStateA;
    private IEnumerator co_action;
    private IEnumerator co_checkStateB;



    private void Awake()
    {
        var player = GameObject.FindGameObjectWithTag("PLAYER");
        if (player != null)
        {
            playerTr = player.GetComponent<Transform>();
        }
        monsterTr = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        moveAgent = GetComponent<MoveAgent>();

        co_checkStateA = Co_CheckStateA();
        co_checkStateB = Co_CheckStateB();
        co_action = Co_Action();



    }
    void Start()
    {
        if (eType == eAIType.TYPE_A)
        {
            StartCoroutine(co_checkStateA);
            StartCoroutine(co_action);
        }
        else
        {
            eState = eMonsterState.IDLE;
            StartCoroutine(co_checkStateB);
            StartCoroutine(co_action);

        }
    }
    public IEnumerator Co_CheckStateA()
    {

        while (true)
        {

            if (eState == eMonsterState.DIE)
            {
                yield break;
            }
            if (isFindPlayer)
            {
                float distance = Vector3.Distance(playerTr.position, monsterTr.position);

                if (!isBlindWall)
                {
                    if (distance <= attackDist)
                    {
                        eState = eMonsterState.ATTACK;
                    }
                    else if (distance <= traceDist)
                    {
                        eState = eMonsterState.TRACE;
                        target = playerTr;
                    }
                    else if (distance >= missPlayerDist)
                    {
                        if (!moveAgent.IsPatrolling)
                        {
                            moveAgent.StartMove();
                            eState = eMonsterState.PATROL;
                        }
                        else if(eState == eMonsterState.IDLE)
                        {
                            eState = eMonsterState.PATROL;
                        }
                        else
                        {
                            yield return null;
                        }
                        
                        /*else
                        {
                            eState = eMonsterState.IDLE;
                        }*/
                        target = null;
                    }
                }
                else
                {
                    if (target != null)
                    {
                        eState = eMonsterState.TRACE;
                    }
                    else
                    {
                        eState = eMonsterState.PATROL;
                    }
                }
            }
            else
            {
                if (moveAgent.IsPatrolling && isReturnPatrol)
                {
                    eState = eMonsterState.PATROL;
                }
                else if(moveAgent.IsPatrolling && !isReturnPatrol)
                {
                    eState = eMonsterState.PATROL;
                }
                else if (!moveAgent.IsPatrolling && !isReturnPatrol)
                {
                    eState = eMonsterState.MISSPLAYER;
                }
                else if (!moveAgent.IsPatrolling && isReturnPatrol)
                {
                    eState = eMonsterState.IDLE;
                }
            }

            yield return new WaitForSeconds(0.1f);
        }

    }
    IEnumerator Co_CheckStateB()
    {
        while (true)
        {
            if (eState == eMonsterState.DIE)
            {
                yield break;
            }
            float distance = Vector3.Distance(playerTr.position, monsterTr.position);
            float distanceForReturnPoint = Vector3.Distance(monsterTr.position, returnPoint.position);
            if (isFindPlayer)
            {
                if (distance <= attackDist)
                {
                    eState = eMonsterState.ATTACK;
                }
                else
                {
                    eState = eMonsterState.TRACE;
                }
            }
            else
            {
                if (distanceForReturnPoint < 0.5f)
                {
                    eState = eMonsterState.IDLE;
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator Co_Action()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            switch (eType)
            {
                case eAIType.TYPE_A:
                    switch (eState)
                    {
                        case eMonsterState.IDLE:
                            animator.SetBool(hashPatrolling, false);
                            animator.SetBool(hashTracing, false);
                            break;
                        case eMonsterState.PATROL:
                            animator.SetBool(hashTracing, false);
                            animator.SetBool(hashPatrolling, true);
                            break;
                        case eMonsterState.TRACE:
                            animator.SetBool(hashPatrolling, false);
                            animator.SetBool(hashTracing, true);
                            moveAgent.TraceTargetPos = playerTr.position;
                            break;
                        case eMonsterState.ATTACK:
                            moveAgent.StopMove();
                            monsterTr.LookAt(playerTr);
                            animator.SetBool(hashPatrolling, false);
                            animator.SetBool(hashTracing, false);
                            SoundManager.Instance.PlayerAudioClip(12, false, false);
                            animator.SetTrigger(hashAttack);
                            yield return new WaitForSeconds(2.1f);
                            break;
                        case eMonsterState.MISSPLAYER:
                            if (isMissPlayer)
                            {
                                moveAgent.StopMove();
                                animator.SetTrigger(hashMissPlayer);
                                yield return new WaitForSeconds(2.5f);
                                isMissPlayer = false;
                                isReturnPatrol = true;
                                moveAgent.StartMove();
                            }
                            break;


                    }
                    break;
                case eAIType.TYPE_B:
                    switch (eState)
                    {
                        case eMonsterState.IDLE:
                            moveAgent.StopMove();
                            animator.SetBool(hashTracing, false);
                            animator.SetBool(hashReturn, false);
                            isReturn = false;
                            break;
                        case eMonsterState.TRACE:
                            animator.SetBool(hashReturn, false);
                            animator.SetBool(hashTracing, true);
                            isReturn = false;
                            moveAgent.TraceTargetPos = playerTr.position;
                            break;
                        case eMonsterState.ATTACK:
                            moveAgent.StopMove();
                            monsterTr.LookAt(playerTr);
                            animator.SetBool(hashTracing, false);
                            animator.SetTrigger(hashAttack);
                            
                            yield return new WaitForSeconds(2.1f);
                            break;
                        case eMonsterState.MISSPLAYER:
                            if (isMissPlayer)
                            {
                                moveAgent.StopMove();
                                animator.SetTrigger(hashMissPlayer);
                                yield return new WaitForSeconds(2.5f);
                                isMissPlayer = false;
                                isReturn = true;
                            }
                            //transform.LookAt(returnPoint);
                            moveAgent.MoveToReturnPoint = returnPoint.position;
                            animator.SetBool(hashTracing, false);
                            animator.SetBool(hashReturn, true);
                            break;

                    }
                    break;
            }

        }
    }
    public void RotateAI(Transform target)
    {
        var rot = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, 2 * Time.deltaTime);
    }
    // Update is called once per frame
    void Update()
    {
        if (eType == eAIType.TYPE_B && eState == eMonsterState.TRACE)
        {
            RotateAI(playerTr);
        }
        if (eType == eAIType.TYPE_B && eState == eMonsterState.MISSPLAYER && isReturn)
        {
            RotateAI(returnPoint);
        }
        if (eType == eAIType.TYPE_A && eState == eMonsterState.TRACE)
        {
            RotateAI(playerTr);
        }

        if (eType == eAIType.TYPE_A)
        {

            Debug.DrawRay(transform.position, playerTr.position - transform.position, Color.blue);
            if (Physics.Raycast(transform.position, playerTr.position - transform.position, out hit))
            {
                if (hit.transform.gameObject.tag == "Wall")
                {
                    isBlindWall = true;
                }
                else
                {
                    isBlindWall = false;
                }
            }
        }
        if (playerTr.GetComponent<Charactermove>().State == Charactermove.eState.DIE)
        {
            StopCoroutine(co_checkStateA);
            StopCoroutine(co_checkStateB);
            moveAgent.StopMove();
            eState = eMonsterState.IDLE;
        }
    }
}
