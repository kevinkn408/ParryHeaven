using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    bool isFrozen;
    EffectsManager effectsManager;
    public static float versusFreezeAmount = 0.17f;
    Player[] players;
    float gameTimer;
    float playerSelect = 0f;
    float currentTimeScale = 1f;
    Coroutine slowTime;
    bool timeAttack = false;

    private void Start()
    {
        effectsManager = FindObjectOfType<EffectsManager>();
        effectsManager.LevelStartFX();
        players = FindObjectsOfType<Player>();
    }

    public void StartGameFreeze(float time)
    {
        StartCoroutine(GameFreeze(time));
    }

    public IEnumerator GameFreeze(float freezeTime)
    {
        if (isFrozen == false)
        {
            AllPlayerInputEnabled(false);
            isFrozen = true;
            Time.timeScale = 0f;
            yield return new WaitForSecondsRealtime(freezeTime);
            isFrozen = false;
            AllPlayerInputEnabled(true);
            Time.timeScale = 1f;
        }
    }

    public IEnumerator SuperFreeze(float freezeTime)
    {
        //AllPlayerInputEnabled(false);

        //DisableToBeHit();
        if (isFrozen == false)
        {
            isFrozen = true;
            Time.timeScale = 0f;
            yield return new WaitForSecondsRealtime(freezeTime);
            isFrozen = false;
            Time.timeScale = 1f;
            //AllPlayerInputEnabled(true);
        }
    }

    public IEnumerator DeathFreeze(float freezeTime)
    {
        if (isFrozen == false)
        {
            isFrozen = true;
            Time.timeScale = 0f;
            yield return new WaitForSecondsRealtime(freezeTime);
            isFrozen = false;
            Time.timeScale = 1f;
        }
    }

    private void AllPlayerInputEnabled(bool allow)
    {
        foreach (Player player in players)
        {
            player.AllowInput = allow;
        }
    }




    IEnumerator WaitUntilStartReset()
    {
        yield return new WaitForSeconds(2f);
        StartCoroutine(LoadRestart());
        //ResetGame();
    }

    //public void ResetGame()
    //{
    //    StartCoroutine(WaitUntilRestart());
    //}

    IEnumerator LoadRestart()
    {
        FindObjectOfType<FadePanel>().FadeOut();
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("Versus P2");
    }

    public void FreezeGame()
    {
        isFrozen = true;
        Time.timeScale = 0f;
    }

    public void UnfreezeGame()
    {
        isFrozen = false;
        Time.timeScale = 1f;
    }

    public static void LoadTitleMenu()
    {
        SceneManager.LoadScene("Title Menu Screen");
        Time.timeScale = 1f;
    }

    //public void ResetRound()
    //{
    //    StartCoroutine(DelayUntilResetRound());
    //}

    //IEnumerator DelayUntilResetRound()
    //{
    //    yield return new WaitForSeconds(3);
    //    EnableInput();
    //    ScoreManager.PlayerHealth = 100;
    //    ScoreManager.EnemyHealth = 100;
    //    player.ResetPosition();
    //    enemy.ResetPosition();
    //}
    public void FadeOutReset()
    {
        StartCoroutine(FadeOutResetDelay());
    }

    IEnumerator FadeOutResetDelay()
    {
        yield return new WaitForSeconds(1.25f);
        FindObjectOfType<FadePanel>().FadeOut();
    }

    public void ResetRound()
    {
        effectsManager.LevelStartFX();
        var activeProjectiles = FindObjectsOfType<Projectile>();
        foreach(Projectile projectile in activeProjectiles)
        {
            projectile.SubtractChildren();
        }
        AllPlayerInputEnabled(true);
        ScoreManager.PlayerHealth = 100;
        ScoreManager.EnemyHealth = 100;
        foreach(Player player in players)
        {
            player.ResetRound();
            player.AllowInput = true;
        }
    }

    public void StartInputDisable()
    {
        StartCoroutine(InputDisable());
    }

    IEnumerator InputDisable()
    {
        AllPlayerInputEnabled(false);
        yield return new WaitForSeconds(versusFreezeAmount);
        AllPlayerInputEnabled(true);

    }
}
