using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public GameObject focus;
    public Vector3 offset;

    void LateUpdate()
    {
        transform.position = focus.transform.position + offset;
    }
}
