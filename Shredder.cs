using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shredder : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("P1_Projectile") || collision.CompareTag("P2_Projectile"))
        {
            var projectile = FindObjectOfType<Projectile>();
            projectile.SubtractChildren();
        }
    }
}
