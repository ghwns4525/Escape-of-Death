using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OpenDoor : MonoBehaviour
{
    private bool isOpen = false;
    [SerializeField] private Transform door;
    private Quaternion openRot;

    public bool IsOpen { get => isOpen; set => isOpen = value; }

    // Start is called before the first frame update
    void Start()
    {
        openRot = Quaternion.Euler(door.rotation.x, -125, door.rotation.z);
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
    public void RotateDoor()
    {
        door.DOLocalRotate(new Vector3(0,-125,0), 3f);
        isOpen = true;
        SoundManager.Instance.PlayerAudioClip(8, false, false);
    }
}
