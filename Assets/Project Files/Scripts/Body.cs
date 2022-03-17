using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Body : MonoBehaviour
{
    public AudioSource deactivate;
    public AudioSource finish;
    public AudioSource jump;
    public AudioSource dash;

    public GameObject soul;
    public float jumpHeight;
    public float speed;
    public float acceleration;
    public float dashPower;

    float horizontal;
    bool dashing = false;
    float dashInTime;
    float dashDirection = 1;
    public bool canJump;

    Rigidbody rb;

    bool active = false;
    bool dead = false;

    Bound bound;
    public string boundString;

    Renderer ren;

    public Material activeMat;
    public Material deadMat;
    public Material jumpPad;

    Manager man;

    bool firstDeath;

    void Start()
    {
        ren = GetComponent<MeshRenderer>();
        dashInTime = 0;
        rb = GetComponent<Rigidbody>();
        bound = GameObject.Find(boundString).GetComponent<Bound>();

        man = GameObject.Find("Manager").GetComponent<Manager>();
    }

    void Update()
    {
        canJump = CanJump();

        if (active)
        {
            if (!bound.FixPosition(transform.position))
            {
                SoulOut();
                Deactivate();
            }
            if (dashing) Dash();
            else
            {
                BodyMove();
                Jump();
            }
        }
    }

    void BodyMove()
    {
        horizontal = Input.GetAxis("Horizontal");

        if (horizontal > 0) dashDirection = 1;
        if (horizontal < 0) dashDirection = -1;

        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, horizontal * speed);

        //rb.velocity += new Vector3(0, 0, acceleration * horizontal * Time.deltaTime);
        //rb.velocity = new Vector3(0, rb.velocity.y, Mathf.Clamp(rb.velocity.z, -speed, speed));

        if (Input.GetKeyDown(KeyCode.LeftShift) && CanDash())
            {
                dashing = true;
                dash.Play();
                rb.mass = 0.1f;
            }
        }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && CanJump())
        {
            jump.Play();
            rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
            //rb.velocity = Vector3.zero;
        }
    }

    

    public void Activate()
    {
        if (!dead)
        {
            active = true;
            //activate.Play();
            ren.material = activeMat;
            rb.mass = 0.5f;
        }
    }

    public void Deactivate()
    {
        if (!firstDeath)
        {
            firstDeath = true;
            man.firstDeath = true;
        }
        active = false;
        //deactivate.Play();
        dead = true;
        ren.material = deadMat;
        rb.mass = 1;
        gameObject.tag = "Dead";
    }

    public void SoulOut()
    {
        GameObject other = Instantiate(soul, transform.position, Quaternion.Euler(Vector3.zero));
        GameObject.Find("Focus").transform.parent = other.gameObject.transform;
        deactivate.Play();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Lava"))
        {
            if (active) SoulOut();
            Deactivate();
        }

        if (collision.gameObject.CompareTag("Spike"))
        {
            if (active) SoulOut();
            Deactivate();
        }

        if (collision.gameObject.CompareTag("Poison"))
        {
            if (active) SoulOut();
            Deactivate();
            gameObject.tag = "JumpPad";
            ren.material = jumpPad;
            gameObject.layer = 29;
        }

        if (collision.gameObject.CompareTag("JumpPad"))
        {
            dash.Play();
            rb.AddForce(Vector3.up * jumpHeight * 1.4f, ForceMode.Impulse);
        }

        if (collision.gameObject.CompareTag("Bullet"))
        {
            if (active) SoulOut();
            Deactivate();
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }

        if (collision.gameObject.CompareTag("Finish") && active)
        {
            StartCoroutine("WaitAndFinish");
        }

        if (collision.gameObject.CompareTag("Coin"))
        {
            Coin coin = collision.gameObject.GetComponent<Coin>();
            coin.other = gameObject;
            if (!coin.droping)
            {
                coin.droping = true;
                coin.aud.Play();
            }

            if (collision.transform.childCount != 0 && collision.transform.GetChild(0).CompareTag("Dash"))
            {
                man.CanDash();
            }
        }
    }

    bool CanJump()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position + Vector3.down * transform.localScale.y * 0.4f, 0.3f);

        foreach(Collider collider in colliders)
        {
            if(collider.gameObject != gameObject && collider.gameObject.layer < 20)
            {
                //Debug.Log(collider.name);
                return true;
            }
        }

        return false;
    }

    void Dash()
    {
        if (dashInTime < 0.3f)
            rb.velocity = new Vector3(0, 0, dashPower * dashDirection);
        else
        {
            dashInTime = 0;
            dashing = false;
            rb.mass = 0.5f;
            man.CannotDash();
        }

        dashInTime += Time.deltaTime;
    }

    bool CanDash()
    {
        return man.canDash;
    }

    IEnumerator WaitAndFinish()
    {
        finish.Play();
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(man.nextScene);
    }
}
