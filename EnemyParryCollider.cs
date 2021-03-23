using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyParryCollider : MonoBehaviour
{
    [SerializeField] AudioClip _parrySound;
    [SerializeField] GameObject parryEffectPosition;
    [SerializeField] GameObject impactParticle;
    [SerializeField] float freezeTime = 0.17f;
    [SerializeField] string projectileTag;
    bool _isFrozen = false;
    public bool objectInTrigger = false;
    GameManager gameManager;
    GameObject currentObject;
    GameObject closestObject = null;
    GameObject closestObjectBase;
    GameObject character;
    //Projectile myClosestObjectBase;
    [SerializeField] GameObject player;
    [SerializeField] bool multiPlayer = false;
    [SerializeField] bool isBot = false;


    public void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag(projectileTag))
        {
            objectInTrigger = true;
            FindClosestObjectInTrigger();
        }
    }

    // going through revelation. All the ontrigger COllider is doing for us is giving us an actual
    // "reference" to the balls that come in, the FIndClosestBall() function FETCHES ITS OWN REFERNECES,
    // AND FINDS THE CLOSEST AKA THE FIRST BASKETBALL
    public void FindClosestObjectInTrigger()
    {
        float distanceToClosestObject = Mathf.Infinity;
        GameObject[] allObjects = GameObject.FindGameObjectsWithTag(projectileTag);
        foreach (GameObject newCurrentObject in allObjects)
        {
            float distanceToObject = (newCurrentObject.transform.position - this.transform.position).sqrMagnitude;
            if (distanceToObject < distanceToClosestObject)
            {
                distanceToClosestObject = distanceToObject;
                closestObject = newCurrentObject;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        objectInTrigger = false;
    }

    //Just use FindGameObjectsWithTag to get an array of all objects with the tag, 
    //then iterate through the array and do GetComponent<Spawner>().Spawn() on each item.

    public void DestroyClosestObject()

    {
        Destroy(closestObject.gameObject);
        if (multiPlayer)
        {
            //FindObjectOfType<Fireball>().FreezeProjectile();
            //closestObject.GetComponentInParent<Projectile>().FreezeProjectile();

        }
        //gameManager.StartInputDisable();
        AudioSource.PlayClipAtPoint(_parrySound, Camera.main.transform.position, 0.5f);
        Instantiate(impactParticle, parryEffectPosition.transform.position, Quaternion.identity);
        objectInTrigger = false;
    }


}
