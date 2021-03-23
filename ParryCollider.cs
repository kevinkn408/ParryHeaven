using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryCollider : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] AudioSource parryVoice;
    [SerializeField] AudioSource parrySFX;
    [SerializeField] AudioSource parryBeep;

    [SerializeField] GameObject parryEffectPosition;
    [SerializeField] GameObject impactParticle;

    BoxCollider2D hurtBox;
    BoxCollider2D parryHitBox;

    GameObject closestProjectile;

    public bool objectInTrigger = false;
    public bool ObjectInTrigger { get { return objectInTrigger; } set { objectInTrigger = value; } }

    Player player;
    Character character;
    
    Coroutine activateParry;
    Coroutine parryActivateWindow;


    public bool isParrying = false;
    bool playerHasParried;
    bool canParry = true;


    public void Start()
    {
        player = GetComponentInParent<Player>();
        //gameObject.tag = player.ProximityTag;
        character = GetComponentInParent<Character>();
        hurtBox = GetComponentInParent<Character>().GetComponent<BoxCollider2D>();
        parryHitBox = GetComponent<BoxCollider2D>();
        parryHitBox.enabled = false;
        gameObject.tag = player.ProximityTag;


    }

    public void AttemptParry()
    {

        // if there is an object detected && character is in IDLE state, ALSO not on cooldown turn on parry collider
        if (objectInTrigger == true && character.currentState == Character.State.Idle && canParry)
        {
            isParrying = false; // I'm using a bool to make sure parry only happens once
            activateParry = StartCoroutine(ActivateParry());
            parryActivateWindow = StartCoroutine(NextParryWindow());
        }
        else
        {
            //if there is no object in trigger / if player mistimes parry
            character.OnParryMiss();
            //StopCoroutine(parryActivateWindow);
            var allFireballs = GameObject.FindGameObjectsWithTag(player.EnemyProjectileTag);
            foreach (GameObject eachFireball in allFireballs)
            {
                eachFireball.GetComponentInParent<Projectile>().ParryMissUnfreeze();
            }
        }
    }

    IEnumerator NextParryWindow()
    {
        canParry = false;
        yield return new WaitForSeconds(0.15f);
        //default 0.14f
        canParry = true;
    }

    //Trigger will destroy first viable object it makes contact with
    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag(player.EnemyProjectileTag) && !isParrying)
        {
            isParrying = true;
            FindClosestObject();
            character.HealthDrainDealer(8);
        }
    }

    IEnumerator ActivateParry()
    {
        float parryWindow = 0;
        playerHasParried = false;

        //Parry Collider stays on until timer ends or a parry is successful
        //Player is invincible during this time because hurtbox is off
        while (parryWindow < GameManager.versusFreezeAmount || playerHasParried == false)
        {
            parryWindow += Time.deltaTime;
            parryHitBox.enabled = true;
            hurtBox.enabled = false;
            yield return null;
        }
        hurtBox.enabled = true;
        parryHitBox.enabled = false;
    }

    // going through revelation. All the ontrigger COllider is doing for us is giving us an actual
    // "reference" to the balls that come in, the FIndClosestBall() function FETCHES ITS OWN REFERNECES,
    // AND FINDS THE CLOSEST AKA THE FIRST BASKETBALL

    public void FindClosestObject()
    {
        //Finds all projectiles in range and finds closest one by recursively comparing ranges with each other
        float positiveInfinity = Mathf.Infinity;
        GameObject[] allProjectiles = GameObject.FindGameObjectsWithTag(player.EnemyProjectileTag);
        foreach (GameObject newCurrentProjectile in allProjectiles)
        {
            float distanceToObject = (newCurrentProjectile.transform.position - this.transform.position).sqrMagnitude;
            if (distanceToObject < positiveInfinity)
            {
                positiveInfinity = distanceToObject;
                closestProjectile = newCurrentProjectile;
            }
        }

        DestroyClosestObject(closestProjectile);

        objectInTrigger = false;
    }

    private void DestroyClosestObject(GameObject closestObject)
    {
        character.OnParry();
        playerHasParried = true;
        var currentClosestObject = closestObject.GetComponentInParent<Projectile>();
        closestObject.SetActive(false);

        currentClosestObject.SubtractChildren();
        //closestObject.GetComponentInParent<Projectile>().SubtractChildren();

        PlayParrySpecialFX();

    }

    private void PlayParrySpecialFX()
    {
        //MakeAllActiveProjectilesFreezable(true);
        //var allFireballs = GameObject.FindGameObjectsWithTag(player.EnemyProjectileTag);
        //foreach(GameObject eachFireball in allFireballs)
        //{
        //    eachFireball.GetComponentInParent<Projectile>().FreezeProjectiles();
        //}
        character.GetComponent<Animator>().Play(character.characterStats.justParryClip, -1, 0f);
        parryVoice.Play();
        parrySFX.Play();
        parryBeep.Play();
        Instantiate(impactParticle, parryEffectPosition.transform.position, Quaternion.identity);
    }
}
