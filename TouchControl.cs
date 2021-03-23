using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControl : MonoBehaviour
{
    Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    player.ChargeStart();
        //}
        //if (Input.GetMouseButtonUp(0))
        //{
        //    player.ChargeRelease();
        //}
    }
}
