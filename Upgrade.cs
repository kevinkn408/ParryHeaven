using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : MonoBehaviour
{
    [SerializeField] Projectile[] allProjectiles = null;
    [SerializeField] GameObject destroyFX;
    [SerializeField] AudioSource destroySFX;
    Character[] characters;
    [SerializeField] GameObject[] skillBoxes;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        TurnPlayerHitBoxesOff();
        StartCoroutine(SlowGradually());
    }

    private void TurnPlayerHitBoxesOff()
    {
        characters = FindObjectsOfType<Character>();
        foreach (Character character in characters)
        {
            character.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    private void OnDisable()
    {
        foreach (Character character in characters)
        {
            character.GetComponent<BoxCollider2D>().enabled = true;
        }

        Time.timeScale = 1f;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator SlowGradually()
    {
        var currentTime = Time.timeScale;

        while (true)
        {
            currentTime -= 0.2f;
            Time.timeScale = currentTime;
            yield return new WaitForSecondsRealtime(0.1f);
            if (currentTime < 0)
            {
                currentTime = 0f;
                Time.timeScale = 0f;
                FindAllActiveObjects();
                StartCoroutine(DestroyAllActiveObjects(allProjectiles));
                yield break;
            }
        }

    }

    private void ActivateBoxes()
    {
        foreach (GameObject skillBox in skillBoxes)
        {
            skillBox.SetActive(true);
        }
    }

    private void FindAllActiveObjects()
    {

        allProjectiles = FindObjectsOfType<Projectile>();
    }

    private IEnumerator DestroyAllActiveObjects(Projectile[] projectilesToDestroy)
    {
        for (int i = 0; i < projectilesToDestroy.Length; i++)
        {
            if (projectilesToDestroy[i].ChildCount > 1)
            {
                var currentProjectile = projectilesToDestroy[i];
                int childCount = currentProjectile.ChildCount;
                for(int x = 0; x < childCount; x++)
                {
                    var positionFX = currentProjectile.SubtractChildren();
                    ObjectDestroyFX(positionFX);
                    currentProjectile.SubtractChildren();
                    yield return new WaitForSecondsRealtime(.15f);
                }
            }
            else
            {
                var positionFX = projectilesToDestroy[i].transform.position;
                ObjectDestroyFX(positionFX);
                projectilesToDestroy[i].SubtractChildren();
            }
            yield return new WaitForSecondsRealtime(0.2f);
        }
        ActivateBoxes();
    }

    //private static void DestroyMultiple(Projectile[] projectilesToDestroy, int i)
    //{
    //    var childCount = projectilesToDestroy[i].ChildCount;
    //    for (i = 0; i < childCount; i++)
    //    {
    //        projectilesToDestroy[i].SubtractChildren();
    //    }
    //}

    private void ObjectDestroyFX(Vector3 position)
    {
        Instantiate(destroyFX, position, Quaternion.identity);
        destroySFX.Play();
    }
}
