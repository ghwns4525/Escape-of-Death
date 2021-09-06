using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawGizmos : MonoBehaviour
{
    public enum Type
    {
        SPHERE,
        CUBE
    }
    [SerializeField] private Color color = Color.yellow;
    [SerializeField] private float radius = 0.9f;
    [SerializeField] private Type type;
    [SerializeField] private Vector3 size;
    private void OnDrawGizmos()
    {
        if(type == Type.SPHERE)
        {
            Gizmos.color = color;
            Gizmos.DrawSphere(transform.position, radius);
        }
        if(type == Type.CUBE)
        {
            Gizmos.color = color;
            Gizmos.DrawCube(transform.position, size);
        }
        
    }
}
