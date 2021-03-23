using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody rb;
    private float ballSpeed = 2f;
    GameObject target;
    Vector2 moveDirection;
    Character player;
    Enemy enemy;
    float delayUntilHurt = 0.1f;
    private bool isProjectileHurting = false;
    [SerializeField] GameObject deathFx;

    float hurtTimer;


    void Start()
    {
        player = FindObjectOfType<Character>();
    }

    // Update is called once per frame
   



    public void Destroy()
    {
        Destroy(gameObject);
        Instantiate(deathFx, transform.position, transform.rotation);

    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy();
            player.Hurt();
            //player.isHurt = true;
            //StartCoroutine(HurtDelay());
        }
        if (other.CompareTag("PlayerProjectile"))
        {
            Destroy();
        }

    }

    IEnumerator HurtDelay()
    {
        //if(isProjectileHurting == true)
        //{
        //    yield break;
        //}
        //isProjectileHurting = true;

        yield return new WaitForSeconds(delayUntilHurt);   
        Destroy();
        player.Hurt();
        //player.isHurt = true;
    }
    //private void StartHurtCounter()
    //{
        //hurtTimer = hurtTimer + Time.fixedDeltaTime;
        ////if (hurtTimer > 0.175f)
        ////{
        ////    //player.Hurt();
        ////    Destroy();
        ////}
        //dog++;
        //Debug.Log(dog);
    //}
}
