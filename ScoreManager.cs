using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static int PlayerHealth = 100;
    public static int EnemyHealth = 100;
    public static int PlayerScore = 0;
    public static int EnemyScore = 0;

    public static int PlayerMeter = 30;
    public static int EnemyMeter = 30;
    [SerializeField] GameObject playerEmblem1;
    [SerializeField] GameObject playerEmblem2;
    [SerializeField] GameObject enemyEmblem1;
    [SerializeField] GameObject enemyEmblem2;
    [SerializeField] GameObject playerWins;
    [SerializeField] GameObject enemyWins;
    GameManager gameManager;
    EffectsManager effectsManager;


    private void Awake()
    {
        StartSingleton();
        PlayerScore = 0;
        EnemyScore = 0;

    }

    private void Start()
    {
        effectsManager = FindObjectOfType<EffectsManager>();
        gameManager = FindObjectOfType<GameManager>();
    }

    private void StartSingleton()
    {
        int numberOfSingletons = FindObjectsOfType<ScoreManager>().Length;
        if (numberOfSingletons > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void EnemyAddToMeter(int meterAmount)
    {
        EnemyMeter += meterAmount;
        if (EnemyMeter < 0)
        {
            EnemyMeter = 0;
        }
        if (EnemyMeter >= 30)
        {
            EnemyMeter = 30;
            //effectsManager.EnemySuperFlash(true);
        }
        //if (EnemyMeter < 30)
        //{
        //    effectsManager.EnemySuperFlash(false);
        //}

    }

    public void PlayerAddToMeter(int meterAmount)
    {
        PlayerMeter += meterAmount;
        if (PlayerMeter < 0)
        {
            PlayerMeter = 0;
        }
        if (PlayerMeter >= 30)
        {
            PlayerMeter = 30;
            //effectsManager.PlayerSuperFlash(true);
        }
        //if (PlayerMeter < 30)
        //{
        //    //effectsManager.PlayerSuperFlash(false);
        //}
    }


    public void AddPlayerScore()
    {
        PlayerScore += 1;
        Debug.Log(PlayerScore);
    }
    public void AddEnemyScore()
    {
        EnemyScore += 1;
        //if (EnemyScore == 2)
        //{
        //    SceneLoader.LoadTitleMenu();
        //}
    }

    public static void ResetValues()
    {
        PlayerScore = 0;
        EnemyScore = 0;
        PlayerMeter = 0;
        EnemyMeter = 0;
        PlayerHealth = 100;
        EnemyHealth = 100;
    }



    public void UpdateEmblem()
    {
        StartCoroutine(UpdateEmblemDelay());
    }

    IEnumerator UpdateEmblemDelay()
    {
        yield return new WaitForSeconds(2.5f);
        switch (PlayerScore)
        {
            //case 0:
                //playerEmblem1.gameObject.SetActive(false);
                //playerEmblem2.gameObject.SetActive(false);
                //break;
            case 1:
                playerEmblem1.gameObject.SetActive(true);
                playerEmblem2.gameObject.SetActive(false);
                break;
            case 2:
                playerEmblem1.gameObject.SetActive(true);
                playerEmblem2.gameObject.SetActive(true);
                StartCoroutine(PlayerWinScreen());
                break;
        }
        switch (EnemyScore)
        {
            //case 0:
                //enemyEmblem1.gameObject.SetActive(false);
                //enemyEmblem2.gameObject.SetActive(false);
                //break;
            case 1:
                enemyEmblem1.gameObject.SetActive(true);
                enemyEmblem2.gameObject.SetActive(false);
                break;
            case 2:
                enemyEmblem1.gameObject.SetActive(true);
                enemyEmblem2.gameObject.SetActive(true);
                StartCoroutine(EnemyWinScreen());
                break;
        }
        if (PlayerScore != 2 && EnemyScore != 2)
        {
            gameManager.FadeOutReset();
        }
    }

    IEnumerator PlayerWinScreen()
    {
        yield return new WaitForSeconds(2);
        playerWins.gameObject.SetActive(true);
    }
    IEnumerator EnemyWinScreen()
    {
        yield return new WaitForSeconds(2);
        enemyWins.gameObject.SetActive(true);
    }
}
