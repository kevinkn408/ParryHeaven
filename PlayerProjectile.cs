using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
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


    void Start()
    {
        enemy = FindObjectOfType<Enemy>();
    }

    // Update is called once per frame
   
    public void Destroy()
    {
        Destroy(gameObject);
        Instantiate(deathFx, transform.position, transform.rotation);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy();
            enemy.Hurt();
            enemy.isHurt = true;
        }
        if (other.CompareTag("EnemyProjectile"))
        {
            Destroy();
        }
    }
}
