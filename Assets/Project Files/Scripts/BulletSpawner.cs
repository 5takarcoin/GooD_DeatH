using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public GameObject bullet;
    public Vector3 direction;
    public float interval = 1;

    private void Start()
    {
        StartCoroutine("SpawnNow");
    }

    IEnumerator SpawnNow()
    {
        yield return new WaitForSeconds(interval);
        SpawnBullet();
        StartCoroutine("SpawnNow");
    }

    // Update is called once per frame
    void SpawnBullet()
    {
        GameObject yo;
        yo = Instantiate(bullet, transform.position, transform.rotation);
        yo.transform.parent = gameObject.transform;
        yo.GetComponent<Bullet>().direction = direction;
    }
}
