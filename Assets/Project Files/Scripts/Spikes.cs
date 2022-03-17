using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    Rigidbody rb;
    public float speed;
    public float height;

    float current;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        current = transform.position.y + .05f;
    }

    void Update()
    {
        if (transform.position.y <= current) rb.velocity = new Vector3(0, speed, 0);
        else if (transform.position.y >= current + height) rb.velocity = new Vector3(0, -speed, 0);
    }
}
