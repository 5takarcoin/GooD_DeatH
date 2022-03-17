using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bound : MonoBehaviour
{
    public float z1, z2;
    public float y1, y2;
    
    void Start()
    {
        z1 = transform.position.z - transform.localScale.z * 0.5f;
        z2 = transform.position.z + transform.localScale.z * 0.5f;
        y1 = transform.position.y - transform.localScale.y * 0.5f;
        y2 = transform.position.y + transform.localScale.y * 0.5f;
    }

    public bool FixPositionZ(Vector3 pos)
    {

        if (pos.z > z1 && pos.z < z2) return true;
        return false;
    }

    public bool FixPositionY(Vector3 pos)
    {

        if (pos.y > y1 && pos.y < y2) return true;
        return false;
    }

    public bool FixPosition(Vector3 pos)
    {

        if (pos.z > z1 && pos.z < z2 && pos.y > y1 && pos.y < y2) return true;
        return false;
    }

}
