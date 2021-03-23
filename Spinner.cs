using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    [SerializeField] Projectile projectile;
    //Rotational Speed
    [SerializeField] float speed = 50f;
    //Forward Direction
    public bool ForwardX = false;
    public bool ForwardY = false;
    public bool ForwardZ = false;
   
    //Reverse Direction
    public bool ReverseX = false;
    public bool ReverseY = false;
    public bool ReverseZ = false;

    public bool spinning = true;

    private void Start()
    {
        projectile.OnParry += StartStopSpinning;
        projectile.OnParryMiss += StopSpinFreeze;
    }

    void Update ()
    {
        if (spinning)
        {
            //Forward Direction
            if (ForwardX == true)
            {
                transform.Rotate(Time.deltaTime * speed, 0, 0, Space.Self);
            }
            if (ForwardY == true)
            {
                transform.Rotate(0, Time.deltaTime * speed, 0, Space.Self);
            }
            if (ForwardZ == true)
            {
                transform.Rotate(0, 0, Time.deltaTime * speed, Space.Self);
            }
            //Reverse Direction
            if (ReverseX == true)
            {
                transform.Rotate(-Time.deltaTime * speed, 0, 0, Space.Self);
            }
            if (ReverseY == true)
            {
                transform.Rotate(0, -Time.deltaTime * speed, 0, Space.Self);
            }
            if (ReverseZ == true)
            {
                transform.Rotate(0, 0, -Time.deltaTime * speed, Space.Self);
            }
        }

    }

    private void StartStopSpinning()
    {
        spinning = false;
    }

    private void StopSpinFreeze()
    {
        spinning = true;
    }

    private void OnEnable()
    {
        spinning = true;
    }
}