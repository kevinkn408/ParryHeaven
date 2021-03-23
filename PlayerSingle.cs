using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerSingle : MonoBehaviour
{
    Animator animator;
    ParryCollider parryCollider;
    [SerializeField] AudioSource parryVoice;
    [SerializeField] AudioClip hurtSound;
    [SerializeField] AudioClip superSound;

    [SerializeField] GameObject fireBall;
    [SerializeField] GameObject fireBallEx;
    [SerializeField] GameObject fireBallSuper;
    [SerializeField] GameObject fireBallPosition;
    //[SerializeField] GameObject superParticles;
    [SerializeField] Slider superMeterBar;
    [SerializeField] Slider healthMeterBar;
    [SerializeField] GameObject startPosition;
    [SerializeField] bool isFacingRight = true;
    [SerializeField] GameObject superFx;
    [SerializeField] Transform superFxPosition;
    //[SerializeField] float parryFreezeTime = .19f;
    public bool isDead = false;
    bool whiffParry;
    [SerializeField] int health = 100;

    Vector2 getFireBallPosition;
    GameManager gameManager;
    ScoreManager scoreManager;
    Rigidbody2D rb;

    public static bool allowInput = true;
    public static bool canBeHit = true;


    [SerializeField] int cost_FireBallEx = -10;
    [SerializeField] int gain_FireBall = 6;
    [SerializeField] int gain_Parry = 5;
    [SerializeField] int gain_Hit = 3;


    void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        animator = GetComponent<Animator>();
        parryCollider = GetComponentInChildren<ParryCollider>();
        getFireBallPosition = fireBallPosition.transform.position;
        rb = GetComponent<Rigidbody2D>();
        gameManager = FindObjectOfType<GameManager>();
    }


    void Update()
    {
        superMeterBar.value = ScoreManager.PlayerMeter;
        healthMeterBar.value = ScoreManager.PlayerHealth;
        WhiffParry();
        if (Input.GetKeyDown("1"))
        {
            StartShootAnimationEx();
        }
        if (Input.GetKeyDown("2"))
        {
            StartParry();
        }
        if (Input.GetKeyDown("3"))
        {
            StartShootAnimation();
        }
    }

    public void StartShootAnimation()
    {
        if (allowInput)
        {
            animator.SetTrigger("IsFiring");
        }
    }

    public void StartShootAnimationEx()
    {
        if(ScoreManager.PlayerMeter >= 10f && allowInput)
        {
            animator.SetTrigger("IsFiringEx");
        }
        else
        {
            return;
        }
    } 

    public void StartShootAnimationSuper()
    {
        var freezeTime = .75f;
        if (ScoreManager.PlayerMeter >= 30f && allowInput)
        {
            Instantiate(superFx, superFxPosition.position, transform.rotation);
            AudioSource.PlayClipAtPoint(superSound, Camera.main.transform.position, 0.5f);
            animator.SetTrigger("IsFiringSuper");
            StartCoroutine(gameManager.SuperFreeze(freezeTime));
            StartCoroutine(AnimationScaleUpdate(freezeTime));
        }
    }

    private void LaunchFireBall()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("ryu_hurt"))
        {
            scoreManager.PlayerAddToMeter(gain_FireBall);
            if (isFacingRight)
            {
                Instantiate(fireBall, getFireBallPosition, transform.rotation);
            }
            else
            {
                Instantiate(fireBall, getFireBallPosition, Quaternion.Euler(0f, 180f, 0f));
            }
        }
    }

    private void LaunchFireBallEx()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("ryu_hurt"))
        {
            scoreManager.PlayerAddToMeter(cost_FireBallEx);;
            if (isFacingRight)
            {
                Instantiate(fireBallEx, getFireBallPosition, transform.rotation);
            }
            else
            {
                Instantiate(fireBallEx, getFireBallPosition, Quaternion.Euler(0f, 180f, 0f));
            }
        }
    }

    private void LaunchFireBallSuper()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("ryu_hurt"))
        {
            scoreManager.PlayerAddToMeter(-100);
            if (isFacingRight)
            {
                //Instantiate(superParticles, Camera.main.transform.position, transform.rotation);

                Instantiate(fireBallSuper, getFireBallPosition, transform.rotation);
            }
            else
            {

                Instantiate(fireBallSuper, getFireBallPosition, Quaternion.Euler(0f,180f,0f));
            }
        }
    }

    public void StartParry()
    {
        var freezeTime = 0.13f;
        if (allowInput)
        {
            if (parryCollider.objectInTrigger == true && whiffParry == false && allowInput)
            {
                //scoreManager.PlayerAddToMeter(gain_Parry);
                //parryCollider.FindClosestObjectInTrigger();
                //parryCollider.DestroyClosestObject();
                animator.Play("ryu_justParry", -1, 0f);

                //animator.SetTrigger("IsJustParrying");
                parryVoice.Play();
                StartCoroutine(gameManager.GameFreeze(freezeTime));
                StartCoroutine(AnimationScaleUpdate(freezeTime));
            }
            else
            {
                animator.SetTrigger("IsParryingMiss");
            }
        }
       
    }


    public void Hurt()
    {
        if (canBeHit)
        {
            //scoreManager.EnemyAddToMeter(gain_Hit);
            animator.Play("ryu_hurt", -1, 0f);
            TurnOffJustParryAnimation();
            AudioSource.PlayClipAtPoint(hurtSound, Camera.main.transform.position, 1f);
            ScoreManager.PlayerHealth -= 0;
            
            if (ScoreManager.PlayerHealth <= 0)
            {
                animator.updateMode = AnimatorUpdateMode.Normal;
                StartCoroutine(gameManager.DeathFreeze(1f));
                rb.velocity = new Vector3(-10f, 0f, 0f);
                FindObjectOfType<EffectsManager>().PlayDeathFX(true);
                FindObjectOfType<ScoreManager>().AddEnemyScore();
                //scoreManager.UpdateEmblem();

            }
        }
       
    }
    void TurnOffJustParryAnimation()
    {
        animator.SetBool("IsParrying", false);
    }

    private void WhiffParry()
    {
        //shadoken said dont do this. unecessary checks.
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("ryu_parry") || animator.GetCurrentAnimatorStateInfo(0).IsName("ryu_hurt"))
        {
            whiffParry = true;
        }
        else
        {
            whiffParry = false;
        }
    }


    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (other.CompareTag("EnemyProjectile"))
    //    {
    //        Hurt();
    //    }
    //}
    IEnumerator AnimationScaleUpdate(float freezeTime)
    {
        animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        yield return new WaitForSeconds(freezeTime);
        animator.updateMode = AnimatorUpdateMode.Normal;

    }
    //private void RoundRestart()
    //{
    //    StartCoroutine(ResetPosition());
    //}
    //IEnumerator ResetPosition()
    //{
    //    yield return new WaitForSeconds(4);
    //    {
    //        Enemy.allowInput = true;
    //        allowInput = true;
    //        FindObjectOfType<FadePanel>().FadeOut();
    //        rb.velocity = new Vector3(0f, 0f, 0f);
    //        gameObject.transform.position = new Vector3(startPosition.transform.position.x, startPosition.transform.position.y, startPosition.transform.position.z);
    //    }
    //}
    public void ResetPosition()
    {
        rb.velocity = new Vector3(0f, 0f, 0f);
        gameObject.transform.position = new Vector3(startPosition.transform.position.x, startPosition.transform.position.y, startPosition.transform.position.z);
    }

    public void HitStunOn()
    {
        allowInput = false;
    }
    public void HitStunOff()
    {
        allowInput = true;
    }
}
