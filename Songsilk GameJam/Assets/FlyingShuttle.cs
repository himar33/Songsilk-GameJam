using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingShuttle : MonoBehaviour
{
    public GameObject bullet;
    public Transform bulletPosition;
    public float bulletSpeed;
    public float minDelay;
    public float maxDelay;
    bool canShoot = true;
    
    void Update()
    {
        if (canShoot)
        {
            StartCoroutine(Shoot());
        }
    }

    IEnumerator Shoot()
    {
        GameObject newBullet = GameObject.Instantiate(bullet, bulletPosition.transform.position, this.transform.rotation);
        newBullet.GetComponent<Rigidbody>().velocity = this.transform.forward * bulletSpeed;
        canShoot = false;
        float delay = Random.Range(minDelay, maxDelay);
        yield return new WaitForSeconds(delay);
        canShoot = true;
    }
}
