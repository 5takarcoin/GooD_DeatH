using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public AudioSource aud;

    public AudioClip keyDash;

    public GameObject lastAud;

    public float speed;
    public float upSpeed;

    [HideInInspector]
    public bool droping = false;

    [HideInInspector]
    public Vector3 target;
    [HideInInspector]
    public GameObject other;

    public float currentSpeed = -20;

    Manager man;

    private void Start()
    {
        man = GameObject.Find("Manager").GetComponent<Manager>();

        if (transform.childCount != 0) aud.clip = keyDash;
    }

    void Update()
    {
        if (droping)
            Drop();
        else
            transform.Rotate(0, speed * Time.deltaTime * 90, 0);
    }


    void Drop()
    {
        currentSpeed += upSpeed;
        transform.position = Vector3.MoveTowards(transform.position, other.transform.position, currentSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, other.transform.position) < 0.1f)
        {
            man.coins++;
            man.PlayLastCoinAudio();
            Destroy(gameObject);
        }
    }
}
