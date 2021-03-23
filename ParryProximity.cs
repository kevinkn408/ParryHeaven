using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryProximity : MonoBehaviour
{
    public ParryCollider parryCollider;
    Player player;

    
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<Player>();
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag(player.EnemyProjectileTag))
        {
            parryCollider.ObjectInTrigger = true;
        }
    }

}
