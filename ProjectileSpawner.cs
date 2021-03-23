using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float rateMin = 0.25f;
    [SerializeField] float rateMax = 0.75f;

    ObjectPooler objectPooler;
    int randomNumberGenerator;
    Coroutine waitAndShoot;
    float shootRateMin = 2, shootRateMax = 6;
    float shootSpeedMin = 1f, shootSpeedMax = 2f;
    GameObject currentProjectile;



    // Start is called before the first frame update

    void Start()
    {
        objectPooler = GetComponent<ObjectPooler>();

        Flip();
        Invoke("ShootCoroutine", 2f);
    }

    void ShootCoroutine()
    {
        waitAndShoot = StartCoroutine(WaitAndShoot());
        StartCoroutine(WaitShootSuper());
        StartCoroutine(WaitShootEx());
    }

    void Flip()
    {
        transform.localScale =
        new Vector3(transform.localScale.x * -1,
        transform.localScale.y,
        transform.localScale.z);
    }

    // Update is called once per frame

    IEnumerator WaitAndShoot()
    {
        while (true)
        {
            var hasShot = false;
            if (!hasShot)
            {
                Shoot();
                hasShot = true;
            }
            yield return new WaitForSeconds(Random.Range(rateMin, rateMax));
            //yield return new WaitForSeconds(.25f);

        }
    }

    public void StopShoot()
    {
        StopCoroutine(waitAndShoot);
    }


    void Shoot()
    {

        //var randomNum = Random.Range(0, 100);
        //if(randomNum < 80)
        //{
        //    currentProjectile = objectPooler.GetFireBall();
        //}
        //else if(randomNum >= 80 && randomNum < 90)
        //{
        //    currentProjectile = objectPooler.GetEx();
        //}
        //else if(randomNum >= 90)
        //{
        //    //currentProjectile = objectPooler.GetSuper();
        //}

        //var projectile = currentProjectile;

        Launch();

        //Launch(objectPooler.GetSuper());
    }

    void Launch()
    {
        var projectile = objectPooler.GetFireBall();
        projectile.transform.position = target.transform.position;
        projectile.GetComponent<Projectile>().IsIndependent = true;
        projectile.GetComponent<Projectile>().ProjectileSpeed = Random.Range(1f,3f);
        projectile.SetActive(true);
    }


    IEnumerator WaitShootSuper()
    {
        while (true)
        {
            yield return new WaitForSeconds(45);
            var projectile = objectPooler.GetSuper();
            projectile.transform.position = target.transform.position;
            projectile.GetComponent<Projectile>().IsIndependent = true;
            projectile.GetComponent<Projectile>().ProjectileSpeed = 3f;
            projectile.SetActive(true);
        }

    }

    IEnumerator WaitShootEx()
    {
        while (true)
        {
            yield return new WaitForSeconds(10);
            var projectile = objectPooler.GetEx();
            projectile.transform.position = target.transform.position;
            projectile.GetComponent<Projectile>().IsIndependent = true;
            projectile.GetComponent<Projectile>().ProjectileSpeed = 3f;
            projectile.SetActive(true);
        }

    }
}
