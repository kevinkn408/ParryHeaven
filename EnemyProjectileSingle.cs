using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileSingle : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody rb;
    private float ballSpeed = 2f;
    GameObject target;
    Vector2 moveDirection;
    PlayerSingle player;
    Enemy enemy;
    float delayUntilHurt = 0.1f;
    private bool isProjectileHurting = false;


    void Start()
    {
        // rb = GetComponent<Rigidbody>();
        // target = GameObject.Find("Player");
        // moveDirection = (target.transform.position - transform.position).normalized * ballSpeed;
        // rb.velocity = new Vector2(moveDirection.x, moveDirection.y);
        player = FindObjectOfType<PlayerSingle>();
    }

    // Update is called once per frame
   
    public void Destroy()
    {
        Destroy(gameObject);
        //Instantiate(deathFx, transform.position, transform.rotation);

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(HurtDelay());
        }
        //if (other.CompareTag("PlayerProjectile"))
        //{
        //    Destroy();
        //}

    }

    IEnumerator HurtDelay()
    {
        //if(isProjectileHurting == true)
        //{
        //    yield break;
        //}
        //isProjectileHurting = true;
        yield return new WaitForSeconds(delayUntilHurt);
        player.Hurt();
        //player.isDead = true;
        Destroy();
        //isProjectileHurting = false;
    }
}
