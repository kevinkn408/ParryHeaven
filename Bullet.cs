using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Player player;
    Projectile projectile;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        projectile = GetComponentInParent<Projectile>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player 1"))
        {
            print("Bruh");
            player.GetComponentInChildren<Character>().Hurt();
        }
        if (collision.CompareTag("P1_Proximity"))
        {
            projectile.InFreezeRange = true;
        }
    }
}
