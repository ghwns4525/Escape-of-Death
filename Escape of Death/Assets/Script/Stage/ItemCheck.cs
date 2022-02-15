using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCheck : MonoBehaviour
{

    RaycastHit hit;
    // �νİŸ�
    float rayDistance = 15.0f;
    Key key;
    // ���� UI
    [SerializeField] GameObject InformationKeyUI;
    [SerializeField] GameObject InformationDoorUI;
    [SerializeField] bool isGetKey = false;
    [SerializeField] Text nonKey;
    int mask;

    private bool isNonText = false;
    
    OpenDoor openDoor;
    

    // Start is called before the first frame update
    void Start()
    {
        key = FindObjectOfType<Key>();
        openDoor = FindObjectOfType<OpenDoor>();
        mask = (1 << LayerMask.NameToLayer("MonsterArea"));
        mask = ~mask;
    }

    // Update is called once per frame
    void Update()
    {
        RayCheck();
    }

    void RayCheck()
    {
        // ��ü ��ü�� �ν��ϴ� ù��° Raycast �߻�
        if (Physics.Raycast(transform.position, transform.forward, out hit, 100000f,mask))
        {
            Debug.DrawRay(transform.position, transform.forward * 1000f, Color.yellow);
            Debug.Log(hit.collider.gameObject.name);
            // �ι�° Raycast �߻�
            if (Physics.Raycast(transform.position, transform.forward, out hit, rayDistance,mask))
            {
                Debug.DrawRay(transform.position, transform.forward * rayDistance, Color.red);
                // ��� ��ü �� ����, ���� �ν�
                if (hit.collider.gameObject.tag == "KEY")
                {                   
                    InformationKey();
                    ItemPut(hit.collider.gameObject);
                }
                else if (hit.collider.gameObject.name == "Door_C")
                {
                    if (!openDoor.IsOpen)
                    {
                        InformationDoor();
                        if (isGetKey && Input.GetKeyDown(KeyCode.F))
                        {
                            openDoor.RotateDoor();
                            isGetKey = false;                            
                        }
                        else if(!isGetKey && Input.GetKeyDown(KeyCode.F))
                        {
                            if(!isNonText)
                            {
                                isNonText = true;
                                nonKey.gameObject.SetActive(true);
                                StartCoroutine("NonKey");
                            }
                        }
                    }
                    else
                    {
                        InformationDoorUI.SetActive(false);
                    }
                }                
                else
                {
                    InformationOff();
                }
            }
            else
            {
                InformationOff();
            }
        }
    }

    void InformationDoor()
    {        
        InformationDoorUI.SetActive(true);
    }

    void InformationKey()
    {
        key.IsLooking = true;
        InformationKeyUI.SetActive(true);
    }


    void ItemPut(GameObject gameObject)
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            Destroy(gameObject);
            isGetKey = true;
        }       
    }

    void InformationOff()
    {    
       
         key.IsLooking = false;
         InformationKeyUI.SetActive(false);
         InformationDoorUI.SetActive(false);
    }

    IEnumerator NonKey()
    {
        nonKey.transform.Translate(Vector2.up * Time.deltaTime);
        yield return new WaitForSeconds(2.0f);
        nonKey.gameObject.SetActive(false);
        isNonText = false;
    }
}
