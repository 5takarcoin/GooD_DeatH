using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soul : MonoBehaviour
{
    public AudioSource activate;
    public AudioSource cant;
    public AudioSource door;

    float horizontal;
    float vertical;

    public float speed;

    bool starting = true;
    bool active = true;
    Collider ot;

    public string boundString;
    GameObject bound;
    Bound bnd;
    Vector3 target;

    bool caseHandle = false;

    Manager man;

    private void Start()
    {
        Time.timeScale = 1.5f;

        man = GameObject.Find("Manager").GetComponent<Manager>();

        target = transform.position + Vector3.up;
        bound = GameObject.Find(boundString);
        bnd = bound.GetComponent<Bound>();
    }

    void Update()
    {
        if(!caseHandle) CaseHandle(bnd, 5);

        if (starting)
        {
            Animo(target);
        }
        else if (active)
        {
            if (caseHandle)
            {
                cant.Play();
                Animo(target);
            }
            else SoulMove();
        }
        else Transition(ot);
    }

    void SoulMove()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(0, vertical, horizontal).normalized * speed * Time.deltaTime;

        transform.Translate(move);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Body"))
        {
            active = false;
            activate.Play();
            ot = other;
        }

        if (other.gameObject.CompareTag("Coin"))
        {
            Coin coin = other.gameObject.GetComponent<Coin>();
            coin.other = gameObject;
            if (!coin.droping)
            { 
                coin.droping = true;
                coin.aud.Play();
            }

            if (other.transform.childCount != 0 && other.transform.GetChild(0).CompareTag("Key"))
            {
                man.HaveKey();
            }

            if (other.transform.childCount != 0 && other.transform.GetChild(0).CompareTag("Dash"))
            {
                man.CanDash();
            }
        }

        if (other.gameObject.CompareTag("Door"))
        {
            if (!HaveKey())
            {
                caseHandle = true;
                GameObject door = other.gameObject;
                Bound doorBound = door.GetComponent<Bound>();
                CaseHandleReverse(doorBound, 2);
                man.noKey = true;
                //Debug.Log("Soul: " + man.noKey);
            }

            else
            {
                door.Play();
                Destroy(other.gameObject);
                man.DontHaveKey();
            }
        }
    }

    void Transition(Collider other)
    {
        transform.position = Vector3.MoveTowards(transform.position, other.transform.position, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, other.transform.position) < 0.1f)
        {
            Destroy(gameObject);
            GameObject.Find("Focus").transform.parent = other.gameObject.transform;
            other.gameObject.GetComponent<Body>().Activate();
        }
    }

    void Animo(Vector3 target)
    {
        if (Vector3.Distance(transform.position, target) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        }
        else { 
            starting = false;
            caseHandle = false;
        }
    }

    void CaseHandle(Bound bnd, float dist)
    {
        if(!bnd.FixPositionZ(transform.position))
        {
            caseHandle = true;
            float dir = bound.transform.position.z - transform.position.z > 0 ? 1 : -1;
            target = transform.position + Vector3.forward * dir * dist;
            man.far = true;
        }

        if (!bnd.FixPositionY(transform.position))
        {
            caseHandle = true;
            float dir = bound.transform.position.y - transform.position.y > 0 ? 1 : -1;
            target = transform.position + Vector3.up * dir * dist;
            man.far = true;
        }
    }

    void CaseHandleReverse(Bound doorBound, float dist)
    {
        if (transform.position.z < doorBound.z1) target = transform.position + Vector3.back * dist;
        if (transform.position.z > doorBound.z2) target = transform.position + Vector3.forward * dist;
        if (transform.position.y < doorBound.y1) target = transform.position + Vector3.down * dist;
        if (transform.position.y > doorBound.y2) target = transform.position + Vector3.up * dist;
    }

    bool HaveKey()
    {
        return man.haveKey;
    }

}
