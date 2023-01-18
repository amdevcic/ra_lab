using UnityEngine;
using System.Collections;

public class MoveCamera : MonoBehaviour
{
    public float orbitSpeed = 2;
    public void LateUpdate()
    {
        if (Input.GetMouseButton(1))
        {
            transform.RotateAround(Vector3.zero, Vector3.up, Input.GetAxis("Mouse X")*orbitSpeed);
        }
    }
}