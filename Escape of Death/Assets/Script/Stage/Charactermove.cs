using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Charactermove : MonoBehaviour
{
    public enum eState
    {
        IDLE,
        WALK,
        BACKMOVE,
        RUN,
        DIE
    }

    private eState state = eState.IDLE;
    [SerializeField]
    private float WalkSpeed;

    GameObject audioObject;
    AudioSource audio;

    private bool isRunAudio = false;
    private bool isStopCheck = false;

    private Transform collidedMonster;

    [SerializeField]
    private float runSpeed;
    [SerializeField] private float applySpeed;

    //상태
    [SerializeField] private bool isRun = false;
    private bool isCollidedWall = false;

    private Animator animator;
    //민감도
    [SerializeField]
    private float lookSensitvity;

    //카메라  회전 한계
    [SerializeField] private float cameraRotationLimit;
    private float currentRotation = 0;
    [SerializeField] private float rotSpeed;
    private float currentRotSpeed = 0;
    private float currentCameraRotationX = 0;
    
    private CameraShake camShake;

    [SerializeField]private GameObject optionObject;
    private bool isPause = false;

    //필요
    [SerializeField]
    private Camera TheCamera;
    private Rigidbody MyRigid;
    private readonly int hashIsWalk = Animator.StringToHash("IsWalk");
    private readonly int hashIsBackWalk = Animator.StringToHash("IsBackWalk");
    private readonly int hashIsRun = Animator.StringToHash("IsRun");

    public bool IsRun { get => isRun; set => isRun = value; }
    public bool IsPause { get => isPause; set => isPause = value; }
    public eState State { get => state; }

    void Start()
    {
        // 모든 카메라를 가져와버려서 삭제
        //TheCamera = FindObjectOfType<Camera>();
        MyRigid = GetComponent<Rigidbody>();
        applySpeed = WalkSpeed;
        animator = GetComponent<Animator>();
        StartCoroutine(Co_Action());
        camShake = GetComponentInChildren<CameraShake>();
        StartCoroutine(camShake.Co_Shake());
        audioObject = GameObject.Find("SoundManager");
        // 바로 스테이지 실행시 주석
        audio = audioObject.GetComponent<AudioSource>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        currentRotSpeed = rotSpeed;
        currentRotation = cameraRotationLimit;
    }


    void Update()
    {
        if (state != eState.DIE)
        {
            TryRun();
            Move();
            CameraRotation();
            CharcterRot();
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OptionPopup();
            }
        }
    }
    private void TryRun()
    {
    if(Input.GetKey(KeyCode.LeftShift))
        {
            Running();
            if(!isRunAudio)
            {
                SoundManager.Instance.StopAudioClip(false, false);
                isRunAudio = true;
            }           
        }
    if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            RunningCancel();
            if(isRunAudio)
            {
                SoundManager.Instance.StopAudioClip(false, false);
                isRunAudio = false;
            }
        }
    }
    //Running
    private void Running()
    {
        if (!isCollidedWall)
        {
            isRun = true;
            applySpeed = runSpeed;
        }
        
    }

    private void RunningCancel ()
    {
        animator.SetBool(hashIsRun, false);
        isRun = false;
        applySpeed = WalkSpeed;
    }

    
    private void CharcterRot()
    {
        transform.Rotate(0f, Input.GetAxis("Mouse X")* currentRotSpeed, 0f, Space.Self);
    }
    
   
    
    private void Move()
    {
        
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        float _moveDirZ = Input.GetAxisRaw("Vertical");
        
        Vector3 _MoveHorizontal = transform.right * _moveDirX;
        Vector3 _MoveVertical = transform.forward * _moveDirZ;

        Vector3 _velocity = (_MoveHorizontal + _MoveVertical).normalized * applySpeed;
        if (_moveDirZ < 0)
        {
            if (isRun)
            {
                state = eState.RUN;                
            }
            else
            {
                state = eState.BACKMOVE;                
            }
           
        }
        else if (_moveDirX != 0 || _moveDirZ != 0)
        {
            if (isRun)
            {
                state = eState.RUN;                         
            }
            else
            {
                state = eState.WALK;               
            }
        }
        else
        {
            state = eState.IDLE;
        }

        // 바로 스테이지 실행시 주석
        if (!isPause)
        {
            if (state == eState.WALK || state == eState.BACKMOVE)
            {
                float vecMagnitude = _velocity.magnitude / WalkSpeed;
                if (vecMagnitude > 0.5f)
                {
                    if (!audio.isPlaying)
                    {
                        SoundManager.Instance.PlayerAudioClip(6, false, false);
                    }
                }
                else
                {
                    SoundManager.Instance.StopAudioClip(false, false);
                }
                Debug.Log(vecMagnitude);
            }
            else if (state == eState.RUN)
            {
                float vecMagnitude = _velocity.magnitude / runSpeed;
                if (vecMagnitude > 0.5f)
                {
                    if (!audio.isPlaying)
                    {
                        SoundManager.Instance.PlayerAudioClip(7, false, false);
                    }
                }
                else
                {
                    SoundManager.Instance.StopAudioClip(false, false);
                }
            }
        }

        // 캐릭터가 멈출때 발소리 사운드 뮤트
        if(_velocity.magnitude == 0)
        {
            if(!isStopCheck)
            {
                SoundManager.Instance.StopAudioClip(false, false);
                isStopCheck = true;
            }
        }
        else
        {
            isStopCheck = false;
        }

        MyRigid.MovePosition(transform.position + _velocity * Time.deltaTime);
        
    }
       
    private void CameraRotation() // 카메라 회전 (상,하)
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotation = _xRotation * lookSensitvity;
        currentCameraRotationX -= _cameraRotation;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -currentRotation, currentRotation);

        TheCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }
    IEnumerator Co_Action()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            switch (state)
            {
                case eState.WALK:
                    animator.SetBool(hashIsRun, false);
                    animator.SetBool(hashIsBackWalk, false);
                    animator.SetBool(hashIsWalk, true);
                    break;
                case eState.BACKMOVE:
                    animator.SetBool(hashIsWalk, false);
                    animator.SetBool(hashIsRun, false);
                    animator.SetBool(hashIsBackWalk, true);
                    break;
                case eState.RUN:
                    animator.SetBool(hashIsWalk, false);
                    animator.SetBool(hashIsBackWalk, false);
                    animator.SetBool(hashIsRun, true);
                    break;
                default:
                    animator.SetBool(hashIsBackWalk, false);
                    animator.SetBool(hashIsWalk, false);
                    animator.SetBool(hashIsRun, false);
                    break;

            }
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Wall")
        {
            isCollidedWall = true;
            applySpeed = 1f;
        }
        
    }
    
   /* private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.tag == "Wall")
        {
            applySpeed = 1f;
        }
    }*/
    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag == "Wall")
        {
            isCollidedWall = false;
            applySpeed = WalkSpeed;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "MONSTER")
        {
            state = eState.DIE;
            collidedMonster = other.transform.GetComponentInParent<MonsterAI>().transform.Find("LookPoint").transform;
            transform.LookAt(collidedMonster);
            TheCamera.transform.localEulerAngles = new Vector3(30f, 0f, 0f);
            Invoke("LoadDeathSceneForInvoke", 0.6f);
        }
        if(other.tag == "MANHOLE")
        {
            SoundManager.Instance.PlayerAudioClip(14, false, false);
        }
        if(other.tag == "IRONBAR")
        {
            SoundManager.Instance.PlayerAudioClip(9, false, false);
        }
        if(other.tag == "BUSH")
        {
            SoundManager.Instance.PlayerAudioClip(16, false, false);
        }
    }
    
    public void OptionPopup()
    {
            if (optionObject.activeSelf == false)
            {
                Time.timeScale = 0f;
                currentRotSpeed = 0;
                currentRotation = 0;
                optionObject.SetActive(true);
                SoundManager.Instance.PlayerAudioClip(4, false, false);
                isPause = true;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Time.timeScale = 1f;
                currentRotSpeed = rotSpeed;
                currentRotation = cameraRotationLimit;
                optionObject.SetActive(false);
                SoundManager.Instance.PlayerAudioClip(4, false, false);
                isPause = false;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }
    private void LoadDeathSceneForInvoke()
    {
        SceneManager.LoadScene("DeathScene");
    }

}
