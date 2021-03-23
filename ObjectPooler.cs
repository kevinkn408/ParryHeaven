using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    //Player player;
    //Character character;


    [SerializeField] int poolSize = 5;

    [SerializeField] GameObject normal;
    [SerializeField] GameObject special;
    [SerializeField] GameObject super;


    List<GameObject> normalProjectilePool = new List<GameObject>();
    List<GameObject> specialProjectilePool = new List<GameObject>();
    List<GameObject> superProjectilePool = new List<GameObject>();

    GameObject nextObject;
    //GameObject nextEx;
    //GameObject nextSuper;


    void Awake()
    {
        //player = GetComponentInParent<Player>();
        //character = GetComponentInParent<Character>();
        PoolFireballs();
        PoolEx();
        PoolSuper();
    }

    private void PoolFireballs()
    {
        for (var i = 0; i < poolSize; i++)
        {
            var pooledObject = Instantiate(normal, transform.position, Quaternion.identity);
            pooledObject.transform.parent = gameObject.transform;
            normalProjectilePool.Add(pooledObject);
            pooledObject.gameObject.SetActive(false);
        }
    }

    private void PoolEx()
    {
        for (var i = 0; i < 5; i++)
        {
            var pooledObject = Instantiate(special, transform.position, Quaternion.identity);
            pooledObject.transform.parent = gameObject.transform;
            specialProjectilePool.Add(pooledObject);
            pooledObject.gameObject.SetActive(false);
        }
    }

    private void PoolSuper()
    {
        for (var i = 0; i < 5; i++)
        {
            var pooledObject = Instantiate(super, transform.position, Quaternion.identity);
            pooledObject.transform.parent = gameObject.transform;
            superProjectilePool.Add(pooledObject);
            pooledObject.gameObject.SetActive(false);
        }
    }

    public GameObject GetFireBall()
    {
        for(var i = 0; i < normalProjectilePool.Count; i++)
        {
            if (!normalProjectilePool[i].activeInHierarchy)
            {
                nextObject = normalProjectilePool[i];
            }
        }
        return nextObject;
    }

    public GameObject GetEx()
    {
        for(var i = 0; i < specialProjectilePool.Count; i++)
        {
            if (!specialProjectilePool[i].activeInHierarchy)
            {
                nextObject = specialProjectilePool[i];
            }
        }
        return nextObject;
    }

    public GameObject GetSuper()
    {
        for (var i = 0; i < superProjectilePool.Count; i++)
        {
            if (!superProjectilePool[i].activeInHierarchy)
            {
                nextObject = superProjectilePool[i];
            }
        }
        return nextObject;
    }

    public void ReturnToPool(GameObject activeObject)
    {
        var activeObjectTransform = activeObject.transform;
        activeObjectTransform.parent = transform;
        activeObjectTransform.localPosition = Vector3.zero;
        activeObjectTransform.gameObject.SetActive(false);

        foreach(Transform child in activeObject.gameObject.transform)
        {
            child.gameObject.SetActive(true);
        }
    }
}
