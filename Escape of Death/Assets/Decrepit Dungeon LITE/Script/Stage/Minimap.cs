using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    [SerializeField] Transform playerTr;
    private float moveSpeed = 5.0f;
    private float firstRotate;


    private void Start()
    {
        firstRotate = playerTr.eulerAngles.y;
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, playerTr.position + new Vector3(0, 10, 0), Time.deltaTime * moveSpeed);
        transform.rotation = Quaternion.Euler(90f , 0f, firstRotate + playerTr.eulerAngles.y);
    }
}