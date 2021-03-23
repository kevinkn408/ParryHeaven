using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Character : MonoBehaviour
{
    public CharacterStats characterStats;

    [Header("Audio Settings")]
    [SerializeField] AudioClip hurtSound;
    [SerializeField] AudioClip superSound;
    [SerializeField] AudioClip exSound;
    [SerializeField] AudioClip chargeSound;

    [Header("Positions")]
    [SerializeField] GameObject fireBallPosition;
    [SerializeField] Transform superFxPosition;
    [SerializeField] GameObject superFx;
    [SerializeField] GameObject chargeFxPos;

    [SerializeField] GameObject chargeFx;

    public enum State { Idle, Dead, Recovering, Parrying };
    public State currentState = State.Idle;

    GameManager gameManager;
    ScoreManager scoreManager;
    EffectsManager effectsManager;

    Rigidbody2D rb;
    Animator animator;
    ParryCollider parryCollider;
    BoxCollider2D characterHurtBox;

    public int currentHealth;
    public int maxHealth;
    public int superMeter;

    SpriteRenderer spriteOrder;

    Player player;

    ObjectPooler objectPooler;

    public int parryScore = 0;
    Coroutine recoveryWindow;

    [SerializeField] float padding = 0.2f;
    float xMin;
    float xMax;

    bool isFrozen = false;
    bool isInCorner = false;

    public delegate void CharacterDelegate();
    //dedicated delegate

    public event CharacterDelegate OnPlayerDeath;
    //event that uses delegate
    public event CharacterDelegate OnPlayerParry;

    public int ParryScore { get { return parryScore; } }

    float exSpeed;
    float fireballSpeed = 1.2f;
    public float FireballSpeed { get { return fireballSpeed; } set { fireballSpeed = value; } }
    float fireballAnimSpeed = 1f;
    public float FireballAnimationSpeed { get { return fireballAnimSpeed; } set { fireballAnimSpeed = value; } }


    private void Awake()
    {
        maxHealth = characterStats.maxHealth;
        currentHealth = maxHealth;
        currentState = State.Idle;
    }

    void Start()
    {
        characterHurtBox = GetComponent<BoxCollider2D>();
        player = GetComponentInParent<Player>();
        parryCollider = GetComponentInChildren<ParryCollider>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        //spriteOrder = GetComponent<SpriteRenderer>();
        effectsManager = FindObjectOfType<EffectsManager>();
        scoreManager = FindObjectOfType<ScoreManager>();
        gameManager = FindObjectOfType<GameManager>();

        objectPooler = transform.parent.GetComponentInChildren<ObjectPooler>();
        transform.localScale = new Vector3(transform.localScale.x * player.Direction, transform.localScale.y, transform.localScale.z);

        superMeter = player.SuperMeter;
        StartCoroutine(DecreaseHealth());
        SetUpBoundaries();


    }

    void Update()
    {
        //getFireBallPosition = fireBallPosition.transform.position;
        if (currentState != State.Dead)
        {
            Move();
        }
    }

    void SetUpBoundaries()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;
    }

    public void Move()
    {
        var newXPos = Mathf.Clamp(transform.position.x, xMin, xMax);
       
        transform.position = new Vector2(newXPos, transform.position.y);
        if(transform.position.x <= xMin || transform.position.x >= xMax)
        {
            isInCorner = true;
        }
        else
        {
            isInCorner = false;
        }
    }

    IEnumerator RecoveryWindow(float recovery)
    {
        currentState = State.Recovering;
        yield return new WaitForSeconds(recovery);
        if(currentState == State.Dead)
        {
            yield break;
        }
        currentState = State.Idle;
    }

    public void StartParry()
    {
        if (currentState == State.Idle)
        {
            parryCollider.AttemptParry();
        }
    }

    private IEnumerator DecreaseHealth()
    {
        while(true)
        {
            HealthDrainDealer(-5);
            yield return new WaitForSeconds(.5f);
        }
    }

    public int OnParry()
    {
        parryScore++;
        OnPlayerParry?.Invoke();
        SuperMeterDealer(1);
        return parryScore;
    }

    public void OnParryMiss()
    {
        StartCoroutine(RecoveryWindow(0.3f));
        animator.SetTrigger("IsParryingMiss");
    }

    public void StartShootAnimation()
    {
        if(currentState == State.Idle)
        {
            animator.SetTrigger("IsFiring");
            animator.SetFloat("AnimationSpeed", 1f * fireballAnimSpeed + player.ShootMashing * 0.1f);
            //animator.SetFloat("AnimationSpeed", 1 - (player.ShootMashing * 0.1f));
       }
    }

    public void StartShootAnimationEx(float speed)
    {
        if(currentState == State.Idle && currentHealth >= 25)
        {
            StartCoroutine(RecoveryWindow(0.1633f));
            animator.SetTrigger("IsFiringEx");
        }
        else { return; }


        exSpeed = speed;
    }

    public void StartShootAnimationSuper()
    {
        if (currentState == State.Idle && currentHealth >= 70)
        {
            player.AllowInput = false;
            StartCoroutine(RecoveryWindow(1.75f));
            Instantiate(superFx, superFxPosition.position, transform.rotation);
            AudioSource.PlayClipAtPoint(superSound, Camera.main.transform.position, 0.5f);
            animator.SetTrigger("IsFiringSuper");
            StartCoroutine(gameManager.SuperFreeze(0.75f));
            StartCoroutine(AnimationScaleUpdate(0.25f));
            effectsManager.ScreenDimmerActivate();

        }
        else { return; }
    }

    private void LaunchFireBall()
    {
        print(fireballSpeed);
        currentHealth += 15;
        player.UpdateHealth();
        SuperMeterDealer(2);
        var speedMultiplier = 1f;
        if (player.ShootMashing <= 0.3f)
        {
            speedMultiplier = 0.05f;
        }
        else if(player.ShootMashing > 0.3f && player.ShootMashing <= 0.5f)
        {
            speedMultiplier = 0.8f;
        }
        else if(player.ShootMashing > 0.5f)
        {
            speedMultiplier = 1.3f;
        }
        var projectile = objectPooler.GetFireBall();
        projectile.transform.position = fireBallPosition.transform.position;
        projectile.GetComponent<Projectile>().ProjectileSpeed = fireballSpeed + player.ShootMashing;
        projectile.SetActive(true);
    }

    private void LaunchFireBallEx()
    {
        //transform.position = new Vector3(transform.position.x + (0.1f * player.Direction), transform.position.y, transform.position.z);
        //otherPlayer.ShootMashing *= 0.3f;
        HealthDrainDealer(-25);
        SuperMeterDealer(-4);
        var projectile = objectPooler.GetEx();
        projectile.transform.position = fireBallPosition.transform.position;
        projectile.GetComponent<Projectile>().ProjectileSpeed = exSpeed;
        exSpeed = 0f;
        projectile.SetActive(true);
    }

    private void LaunchFireBallSuper()
    {
        StartCoroutine(RecoveryWindow(1f));
        currentHealth -= 70;
        player.UpdateHealth();
        player.AllowInput = true;
        var projectile = objectPooler.GetSuper();
        projectile.transform.position = fireBallPosition.transform.position;
        projectile.GetComponent<Projectile>().ProjectileSpeed = 2.8f;
        projectile.SetActive(true);
    }

    //public void SortingOrderNormal()
    //{
    //    spriteOrder.sortingOrder = -1;
    //}

    public void Hurt()
    {
        if (currentState == State.Idle || currentState == State.Recovering && currentHealth > 0)
        {
            player.ButtonUp();
            player.ShootMashing = 0;
            animator.SetTrigger("IsHurt");

            TurnOffJustParryAnimation();
            AudioSource.PlayClipAtPoint(hurtSound, Camera.main.transform.position, 1f);
            currentHealth -= 50;
            player.UpdateHealth();
            recoveryWindow = StartCoroutine(RecoveryWindow(0.17f));

            if (currentHealth <= 0)
            {
                StartCoroutine(gameManager.DeathFreeze(1f));
                currentState = State.Dead;
                isInCorner = false;
                currentHealth = 0;
                //animator.updateMode = AnimatorUpdateMode.Normal;
                //characterHurtBox.enabled = false;
                GiveEnemyScore();
                DeathEffect();
                scoreManager.UpdateEmblem();
                OnPlayerDeath?.Invoke();
            }
            else if (currentState != State.Dead)
            {
                StartCoroutine(HitFreeze());
            }
        }
    }

    IEnumerator HitFreeze()
    {
        if (!isFrozen)
        {
            isFrozen = true;
            Time.timeScale = 0f;
            yield return new WaitForSecondsRealtime(0.1f);
            Time.timeScale = 1f;
            isFrozen = false;
        }
    }

    public void OnEnemyParry(float amount)
    {
        player.ShootMashing -= amount;
        if(player.ShootMashing <= 0)
        {
            player.ShootMashing = 0f;
        }
    }

    void DeathEffect()
    {
        StartCoroutine(DeathFlyOff());
        if (player.IsFacingRight)
        {
            FindObjectOfType<EffectsManager>().PlayDeathFX(true);
        }
        else
        {
            FindObjectOfType<EffectsManager>().PlayDeathFX(false);
        }

    }

    IEnumerator DeathFlyOff()
    {
        rb.velocity = new Vector3(-10f * player.Direction, 0f, 0f);
        yield return new WaitForSeconds(0.2f);
        rb.velocity = new Vector3(0f, 0f, 0f);

    }

    void TurnOffJustParryAnimation()
    {
        //animator.SetBool("IsParrying", false);
    }

    IEnumerator AnimationScaleUpdate(float parryFreezeTime)
    {
        animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        yield return new WaitForSeconds(parryFreezeTime);
        animator.updateMode = AnimatorUpdateMode.Normal;
    }

    public void ResetPosition()
    {
        characterHurtBox.enabled = true;
        currentState = State.Idle;
        currentHealth = maxHealth;
        player.UpdateHealth();
        rb.velocity = new Vector3(0f, 0f, 0f);
        gameObject.transform.position = new Vector3(player.StartingPosition.x, player.StartingPosition.y, player.StartingPosition.z);
    }


    private void SuperMeterDealer(int amount)
    {
        superMeter += amount;
        if(superMeter >= characterStats.maxSuperMeter)
        {
            superMeter = characterStats.maxSuperMeter;
            player.SuperFlash(true);
        }
        else
        {
            player.SuperFlash(false);
        }
        if (superMeter < 0)
        {
            superMeter = 0;
        }
        player.UpdateSuperMeter();
    }

    public void HealthDrainDealer(int amount)
    {
        currentHealth += amount;
        player.UpdateHealth();

        if (currentHealth <= 0)
        {
            currentHealth = 10;
        }

        if(currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    void GiveEnemyScore()
    {
        if (gameObject.tag == "Player 1")
        {
            FindObjectOfType<ScoreManager>().AddEnemyScore();
        }
        else
        {
            FindObjectOfType<ScoreManager>().AddPlayerScore();
        }
    }

    public void ShootChargeCalculator(int amount)
    {
        switch (amount)
        {
            case 0:
                //StartParry();
                break;
            case 1:
                StartShootAnimationEx(2.5f);
                break;
            case 2:
                StartShootAnimationEx(2.65f);
                break;
            case 3:
                StartShootAnimationEx(2.75f);
                break;
            case 4:
                StartShootAnimationEx(4f);
                break;
            case 5:
                StartShootAnimationSuper();
                break;
            case 6:
                StartShootAnimationSuper();
                break;
            default:
                break;
        }
    }

    public void ChargeFX()
    {
        AudioSource.PlayClipAtPoint(chargeSound, Camera.main.transform.position, 0.5f);
        Instantiate(chargeFx, chargeFxPos.transform.position, transform.rotation);

    }

    public void UpgradeWeapon()
    {
        fireballSpeed += 0.5f;
        
    }

}
