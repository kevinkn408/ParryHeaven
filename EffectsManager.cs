using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    [SerializeField] Transform leftSideParticles;
    [SerializeField] Transform rightSideParticles;
    [SerializeField] GameObject deathParticle;
    [SerializeField] AudioClip deathSFX;
    [SerializeField] AudioClip deathVoiceSFX;
    [SerializeField] AudioClip arcadeButtonClick;
    [SerializeField] GameObject playerArrow;
    [SerializeField] GameObject enemyArrow;
    [SerializeField] GameObject playerSuperFlash;
    [SerializeField] GameObject enemySuperFlash;
    [SerializeField] GameObject screenDimmer;
   

    public void ScreenDimmerActivate()
    {
        StartCoroutine(ScreenDimmerSuper(.75f));
    }

    public void ScreenDimmerParry()
    {
        StartCoroutine(ScreenDimmerSuper(0.10f));
    }

    public void ArcadeButtonClick()
    {
        AudioSource.PlayClipAtPoint(arcadeButtonClick, Camera.main.transform.position, 0.4f);
    }

    //public void PlayerSuperFlash(bool isActive)
    //{
    //    if (isActive)
    //    {
    //        playerSuperFlash.gameObject.SetActive(true);
    //    }
    //    else
    //    {
    //        playerSuperFlash.gameObject.SetActive(false);
    //    }
    //}

    public void EnemySuperFlash(bool isActive)
    {
        if (isActive)
        {
            enemySuperFlash.gameObject.SetActive(true);
        }
        else
        {
            enemySuperFlash.gameObject.SetActive(false);
        }
    }

    public void PlayDeathFX(bool isPlayer)
    {
        StartCoroutine(StartDeathFX(isPlayer));
    }

    IEnumerator StartDeathFX(bool isPlayer)
    {
        yield return new WaitForSeconds(.40f);
        if (isPlayer)
        {
            Instantiate(deathParticle, leftSideParticles.position, transform.rotation);
        }
        else
        {
            Instantiate(deathParticle, rightSideParticles.position, Quaternion.Euler(0, 180, 0));

        }
        AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, 0.4f);
        AudioSource.PlayClipAtPoint(deathVoiceSFX, Camera.main.transform.position, .8f);
    }

    public void LevelStartFX()
    {
        StartCoroutine(ArrowTimer());
    }
    IEnumerator ArrowTimer()
    {
        playerArrow.gameObject.SetActive(true);
        enemyArrow.gameObject.SetActive(true);

        yield return new WaitForSeconds(2.5f);
        playerArrow.gameObject.SetActive(false);
        enemyArrow.gameObject.SetActive(false);
    }

    IEnumerator ScreenDimmerSuper(float dimmerTime)
    {
        screenDimmer.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(dimmerTime);
        screenDimmer.gameObject.SetActive(false);

    }
}
