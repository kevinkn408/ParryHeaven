using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    Animator animator;
    EnemyParryCollider parryCollider;
    [SerializeField] AudioSource parryVoice;
    [SerializeField] AudioClip hurtSound;
    [SerializeField] AudioClip superSound;

    [SerializeField] GameObject fireBall;
    [SerializeField] GameObject fireBallEx;
    [SerializeField] GameObject fireBallSuper;
    [SerializeField] GameObject fireBallPosition;
    [SerializeField] GameObject startPosition;
    //[SerializeField] GameObject superParticles;
    [SerializeField] Slider superMeterBar;
    [SerializeField] Slider healthMeterBar;
    [SerializeField] bool isFacingRight = false;
    [SerializeField] GameObject superFx;
    [SerializeField] Transform superFxPosition;


    public bool isHurt = false;
    bool whiffParry;

    Vector2 getFireBallPosition;
    GameManager gameManager;
    ScoreManager scoreManager;
    EffectsManager effectsManager;
    Rigidbody2D rb;

    public static bool allowInput = true;
    public static bool canBeHit = true;

    [SerializeField] int cost_FireBallEx = -10;
    [SerializeField] int gain_FireBall = 6;
    [SerializeField] int gain_Parry = 5;
    [SerializeField] int gain_Hit = 3;


    void Start()
    {
        //allowInput = true;
        //canBeHit = true;
        effectsManager = FindObjectOfType<EffectsManager>();
        scoreManager = FindObjectOfType<ScoreManager>();
        animator = GetComponent<Animator>();
        parryCollider = GetComponentInChildren<EnemyParryCollider>();
        getFireBallPosition = fireBallPosition.transform.position;
        rb = GetComponent<Rigidbody2D>();
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        superMeterBar.value = ScoreManager.EnemyMeter;
        healthMeterBar.value = ScoreManager.EnemyHealth;
        WhiffParry();
    }

    public void StartShootAnimation()
    {
        if (allowInput && !isHurt)
        {
            animator.SetTrigger("IsFiring");
        }
    }

    public void StartShootAnimationEx()
    {
        if (ScoreManager.EnemyMeter >= 10f && allowInput && !isHurt)
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
        if (ScoreManager.EnemyMeter >= 30 && allowInput && !isHurt)
        {
            Instantiate(superFx, superFxPosition.position, transform.rotation);
            AudioSource.PlayClipAtPoint(superSound, Camera.main.transform.position, 0.5f);
            animator.SetTrigger("IsFiringSuper");
            StartCoroutine(gameManager.SuperFreeze(freezeTime));
            StartCoroutine(AnimationScaleUpdate(freezeTime));
            effectsManager.ScreenDimmerActivate();
        }
    }

    private void LaunchFireBall()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("ryu_hurt"))
        {
            scoreManager.EnemyAddToMeter(gain_FireBall);
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
            scoreManager.EnemyAddToMeter(cost_FireBallEx);
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
            scoreManager.EnemyAddToMeter(-100);
            if (isFacingRight)
            {

                Instantiate(fireBallSuper, getFireBallPosition, transform.rotation);
            }
            else
            {
                //Instantiate(superParticles, Camera.main.transform.position, Quaternion.Euler(0f, 180f, 0f));

                Instantiate(fireBallSuper, getFireBallPosition, Quaternion.Euler(0f, 180f, 0f));
            }
        }
    }

    public void StartParry()
    {
        //var freezeTime = 0.19f;

        if (allowInput && !isHurt && !animator.GetCurrentAnimatorStateInfo(0).IsName("ryu_fireball"))
        {
            if (parryCollider.objectInTrigger == true && whiffParry == false)
            {
                StartCoroutine(HurtBoxController());
                scoreManager.EnemyAddToMeter(gain_Parry);
                //parryCollider.FindClosestObjectInTrigger();
                parryCollider.DestroyClosestObject();
                //animator.SetTrigger("IsJustParrying");
                animator.Play("ryu_justParry", -1, 0f);

                parryVoice.Play();
                //StartCoroutine(gameManager.GameFreeze(freezeTime));
                //StartCoroutine(AnimationScaleUpdate(freezeTime));
            }
            else
            {
                animator.Play("ryu_parry", -1, 0f);

                //animator.SetTrigger("IsParryingMiss");
            }
        }
    }


    public void Hurt()
    {
        if (canBeHit)
        {
            animator.SetTrigger("IsHurt");

            scoreManager.PlayerAddToMeter(gain_Hit);
            //animator.Play("ryu_hurt", -1, 0f);
            TurnOffJustParryAnimation();
            AudioSource.PlayClipAtPoint(hurtSound, Camera.main.transform.position, 1f);
            ScoreManager.EnemyHealth -= 48;

            if (ScoreManager.EnemyHealth <= 0)
            {
                allowInput = false;

                animator.updateMode = AnimatorUpdateMode.Normal;
                StartCoroutine(gameManager.DeathFreeze(1f));
                rb.velocity = new Vector3(10f, 0f, 0f);
                FindObjectOfType<EffectsManager>().PlayDeathFX(false);
                FindObjectOfType<ScoreManager>().AddPlayerScore();
                scoreManager.UpdateEmblem();
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

    IEnumerator AnimationScaleUpdate(float parryFreezeTime)
    {
        animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        yield return new WaitForSeconds(parryFreezeTime);
        animator.updateMode = AnimatorUpdateMode.Normal;
    }

    public void ResetPosition()
    {
        rb.velocity = new Vector3(0f, 0f, 0f);
        gameObject.transform.position = new Vector3
        (startPosition.transform.position.x, 
        startPosition.transform.position.y, 
        startPosition.transform.position.z);
    }
    public void HitStunOn()
    {
        isHurt = true;
    }
    public void HitStunOff()
    {
        isHurt = false;
    }
    public void HurtFramesOn()
    {
        canBeHit = true;
    }
    public void HurtFramesOff()
    {
        canBeHit = false;
    }

    IEnumerator HurtBoxController()
    {
        HurtBox(false);
        yield return new WaitForSeconds(0.1f);
        HurtBox(true);
    }

    private void HurtBox(bool isActive)
    {
        if (isActive)
        {
            GetComponent<BoxCollider2D>().enabled = true;
        }
        else
        {
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
