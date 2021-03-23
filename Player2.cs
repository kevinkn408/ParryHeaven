using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour
{
    // Start is called before the first frame update
    public Character player;

    void Start()
    {
        var newPlayer = Instantiate(player, transform.position, Quaternion.identity, transform);
        newPlayer.tag = "Enemy";
    }

    // Update is called once per frame
    void Update()
    {

    }
}
