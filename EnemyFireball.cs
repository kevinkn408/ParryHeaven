using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFireball : MonoBehaviour
{
    [SerializeField] float fireBallSpeed = 1f;
    [SerializeField] public bool isFacingRight = true;
    private bool canBeFrozen = false;
    [SerializeField] bool isPlayerFireBall = true;
    bool isFrozen = false;
    int currentNumberOfChildren;
    bool oneTime = false;
    float originalFireBallSpeed;

    // Start is called before the first frame update
    void Start()
    {
        currentNumberOfChildren = transform.childCount;
        originalFireBallSpeed = fireBallSpeed;
        Destroy(gameObject, 5f);
    }
    // Update is called once per frame
    void Update()
    {
        transform.Translate(fireBallSpeed * Time.deltaTime, 0f, 0f);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (isPlayerFireBall)
        {
            if (collision.CompareTag("EnemyParryCollider"))
            {
                canBeFrozen = true;
            }
        }
        if (!isPlayerFireBall)
        {
            if (collision.CompareTag("PlayerParryCollider"))
            {
                canBeFrozen = true;
            }
        }

    }


    public void FreezeProjectile()
    {
        Debug.Log("Is FROZEN");
        if (canBeFrozen)
        {
            StartCoroutine(StartFreezeProjectile());
        }
    }

    IEnumerator StartFreezeProjectile()
    {
        if (isFrozen == false)
        {
            fireBallSpeed = 0f;
            isFrozen = true;
            yield return new WaitForSeconds(GameManager.versusFreezeAmount);
            isFrozen = false;
            fireBallSpeed = originalFireBallSpeed;
        }
    }
}
