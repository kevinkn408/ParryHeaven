using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sean : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private UnityEngine.GameObject basketball;
    [SerializeField] private UnityEngine.GameObject player;
    [SerializeField] private UnityEngine.GameObject shootPoint;
    [SerializeField] AudioSource _shootSound;
    Rigidbody2D seanRB;
    float fireRate = 2f;
    float nextFire;
    Animator animator;
    bool isGrounded = true;
    float flickSpeed;
    float[] ballSpeedTiming;
    float delayBetweenJumps;
    float jumpForce;
    Coroutine startLaunchBall;
    


    void Awake()
    {
        //change timestep to 0.1667 
        seanRB = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        animator = GetComponent<Animator>();
         //JumpShoot(_ballSet);
    }
   // Update is called once per frame
    
    void Update()
    {
        if (isGrounded == false)
        {
            animator.SetBool("IsJumping", true);

        }
        if (seanRB.velocity.y < 0 && isGrounded == false)
        {
            seanRB.gravityScale = 0.5f;
            animator.SetBool("IsFalling", true);
        }
        else if (isGrounded == true)
        {
            animator.SetBool("IsJumping", false);
            animator.SetBool("IsFalling", false);
        }

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Ceiling")
        {
            startLaunchBall = StartCoroutine(LaunchProjectile(ballSpeedTiming));
        }
       
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Ceiling")
        {
            StopCoroutine(startLaunchBall);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        UnityEngine.GameObject.FindWithTag("floor");
        isGrounded = true;
        seanRB.gravityScale = 1f;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        UnityEngine.GameObject.FindWithTag("floor");
        isGrounded = false;
    }


   IEnumerator LaunchProjectile(float[] ballSpeedTime)
    {
        // StartCoroutine(AnimLaunchProjectile());
        //PUT A FOR LOOP IN HERE TO ITERATE THROUGH EACH BALL.
        for (int i = 0; i <= ballSpeedTime.Length; i++)
        {
            animator.SetBool("IsShooting", true);
            //animator.Play("sean_shoot", -1, 0f);
            _shootSound.Play();
            Vector3 Vo = CalculateVelocity(player.transform.position, shootPoint.transform.position, ballSpeedTime[i]);
            UnityEngine.GameObject ball = Instantiate(basketball, shootPoint.transform.position, Quaternion.identity);
            Rigidbody2D ballRb = ball.GetComponent<Rigidbody2D>();
            ballRb.velocity = Vo;
            Destroy(ball, 5f);
            yield return new WaitForSeconds(flickSpeed);
        }
    }

    private void TurnShootingOff()
    {
        animator.SetBool("IsShooting", false);
    }


    Vector3 CalculateVelocity(Vector3 target, Vector3 origin, float time)
    {
        //define the distance of x and y first
        Vector3 distance = target - origin;
        Vector3 distanceXZ = distance;
        distanceXZ.y = 0f;

        //create a float that represents our distance
        float Sy = distance.y;
        float Sxz = distanceXZ.magnitude;
        //Vy is velocity for y, Vx is velocity for X.
        float Vxz = Sxz / time;
        float Vy = Sy / time + 0.5f * Mathf.Abs(Physics.gravity.y) * time; 

        // Vector3 result = new Vector3(Vxz.x, Vy, Vxz.z);
        Vector3 result = distanceXZ.normalized; //we got rid of Y, itll look like this (1,0,1);
        result = result * Vxz; //(1,0,1) * Vxz
        result.y = Vy;

        return result;
        //could've returned Vector3(Vxz.x, Vy, Vxz.z);
    }

    public void JumpShoot(BallSet _ballSet_) //invoke with shootWait delayparameter?
    {
        Debug.Log("sean is jumping");
        flickSpeed = _ballSet_.flickSpeed;//single float
        jumpForce = _ballSet_.jumpForce;//single float
        ballSpeedTiming = _ballSet_.ballSpeeds;//array
        delayBetweenJumps = _ballSet_.delayUntilJump; //array
        seanRB.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
    }
}
