using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeProximity : MonoBehaviour
{
    Player player;

    void Start()
    {
        player = GetComponentInParent<Player>();
        //gameObject.tag = player.ProximityTag;
    }
}
