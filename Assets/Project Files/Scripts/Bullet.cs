using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector]
    public Vector3 direction;
    Rigidbody rb;

    public AudioSource bull;

    Transform foc;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        foc = GameObject.Find("Focus").transform;
        if(Vector3.Distance(foc.position, transform.position) < 20)
        {
            bull.Play();
        }

    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = direction;
        if (direction.y == 0) transform.rotation = Quaternion.Euler(90, 0, 0);


        if(Vector3.Distance(transform.parent.position, transform.position) > 30)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if(!other.gameObject.CompareTag("Bullet"))
            Destroy(gameObject);
    }
}
