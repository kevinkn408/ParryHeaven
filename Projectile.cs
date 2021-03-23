using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float fireBallSpeed = 1f;
    public float ProjectileSpeed { get { return fireBallSpeed; } set { fireBallSpeed = value; } }
    [SerializeField] float freezeDuration = 0.1f;
    public float FreezeDuration { get { return freezeDuration; } }

    float originalFireBallSpeed;

    ObjectPooler pool;
    Player player;
    GameObject enemy;
    public int childObjectCount;
    bool hasParried;
    float freezeTime;
    public bool canBeFrozen = false;
    public bool Freezable { get { return canBeFrozen; } set { canBeFrozen = value; } }

    bool isRunning = false;

    bool isInRange = false;
    public bool InFreezeRange { set { isInRange = value; } }
    float time;
    Coroutine projectileFreeze = null;

    [SerializeField] bool isIndependent;
    [SerializeField] bool isHoming;

    public bool IsIndependent { get { return isIndependent; } set { isIndependent = value; } }

    public int ChildCount { get { return childObjectCount;  } }
    [SerializeField] Fireball[] allFireballs;

    int childIndex = 0;

    public delegate void ProjectileDelegate();
    public event ProjectileDelegate OnParry;
    public event ProjectileDelegate OnParryMiss;



    private void Start()
    {

        //if (!isIndependent)
        //{
        //    LoadPlayerProjectile();
        //}
        //else
        //{
        //    return;
        //}
        LoadPlayerProjectile();
    }

    private void Awake()
    {
        allFireballs = GetComponentsInChildren<Fireball>();
    }

    private void LoadPlayerProjectile()
    {
        pool = GetComponentInParent<ObjectPooler>();
        childObjectCount = GetComponentsInChildren<Fireball>().Length; //counts the amount of children
        //direction = player.Direction;
        if (!isIndependent)
        {
            player = GetComponentInParent<Player>();
            foreach (Transform child in gameObject.transform)
            {
                child.gameObject.tag = player.ProjectileTag;
            }
        }
        else
        {
            foreach (Transform child in gameObject.transform)
            {
                child.gameObject.tag = "P2_Projectile";
            }
        }

    }

    void FixedUpdate()
    {
        if (isHoming)
        {
            enemy = GameObject.FindWithTag("Player 1");
            transform.position = Vector2.MoveTowards(transform.position, enemy.transform.position, fireBallSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(fireBallSpeed * Time.deltaTime, 0f, 0f);
        }
    }

    private void OnEnable()
    {
        childObjectCount = GetComponentsInChildren<Fireball>().Length;
        originalFireBallSpeed = fireBallSpeed;
        isInRange = false;
        isRunning = false;
    }

    public Vector3 SubtractChildren()
    {
        canBeFrozen = false;
        //var once = true;
        //for (childIndex = 0; childIndex < childObjectCount; childIndex++)
        //{
        //    if (once)
        //    {
        //        once = false;
        //        allFireballs[childIndex].gameObject.SetActive(false);
        //        childObjectCount--;
        //    }

        //}
        Vector3 position = allFireballs[childIndex].transform.position;
        allFireballs[childIndex].gameObject.SetActive(false);
        childIndex++;
        childObjectCount--;
        if (childObjectCount <= 0)
        {
            ReturnToPool();
        }
        return position;

    }

    private void ReturnToPool()
    {
        childIndex = 0;
        pool.ReturnToPool(gameObject);
    }


    public void FreezeProjectiles()
    {
        time = 0f;
        if(isInRange && !isRunning)
        {
            projectileFreeze = StartCoroutine(StartFreezeProjectile());
        }
    }

    IEnumerator StartFreezeProjectile()
    {
        fireBallSpeed = 0f;
        OnParry?.Invoke();

        while (time < freezeDuration)
        {
            time += Time.deltaTime;
            isRunning = true;
            yield return null;
        }
        OnParryMiss?.Invoke();

        isRunning = false;
        fireBallSpeed = originalFireBallSpeed;
    }

    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (other.CompareTag(player.EnemyProximityTag))
    //    {
    //        isInRange = true;
    //    }
    //}

    public void ParryMissUnfreeze()
    {
        if (isRunning)
        {
            OnParryMiss?.Invoke();
            StopCoroutine(projectileFreeze);
        }
        fireBallSpeed = originalFireBallSpeed;
    }
}
