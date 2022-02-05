using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingShuttle : MonoBehaviour
{
    public Transform instantiateAcumulator;
    public GameObject bullet;
    public Transform bulletPosition;
    public AudioSource aSource;
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
        GameObject newBullet = GameObject.Instantiate(bullet, bulletPosition.transform.position, this.transform.rotation,instantiateAcumulator);
        aSource.Play();
        newBullet.transform.Rotate(new Vector3(0, 0, 0));
        newBullet.GetComponent<Rigidbody>().velocity = (-this.transform.right) * bulletSpeed;
        Object.Destroy(newBullet, 4.0f);
        canShoot = false;
        float delay = Random.Range(minDelay, maxDelay);
        yield return new WaitForSeconds(delay);
        canShoot = true;
    }
}
