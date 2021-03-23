using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SinglePlayerEvents : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] ProjectileSpawner projectileSpawner;
    [SerializeField] ScoreKeeper scoreKeeper;
    [SerializeField] SceneLoader sceneLoader;
    [SerializeField] Text countDownText;
    [SerializeField] GameObject restartButton;
    [SerializeField] GameObject gameOverMenu;
    public List<ProjectileSpawner> spawners = new List<ProjectileSpawner>();


    Character character;
    int parryScore;
    // Start is called before the first frame update
    void Start()
    {
        character = player.GetComponentInChildren<Character>();
        parryScore = character.ParryScore;
        character.OnPlayerDeath += StopProjectiles;
        character.OnPlayerDeath += LoadMenu;
        character.OnPlayerParry += UpdateScore;
        //countDownText.gameObject.SetActive(false);
        //restartButton.gameObject.SetActive(false);
        gameOverMenu.gameObject.SetActive(false);


        //adds StopProjectiles() to event so when OnPlayerDeath() runs, so will StopProjectiles();
    }

    void Update()
    {

    }

    void StopProjectiles()
    {
        projectileSpawner.StopShoot();
    }


    void UpdateScore()
    {
        scoreKeeper.GetComponent<TextMeshProUGUI>().text = character.parryScore.ToString();
        StartCoroutine (TextEffect());
        UpdateLevel();
    }

    void UpdateLevel()
    {

    }

    void LoadMenu()
    {
        StartCoroutine(WaitUntilEndGame());
    }

    IEnumerator WaitUntilEndGame()
    {
        var delay = 2;
        Invoke("GameOverMenu", delay);
        yield return new WaitForSeconds(delay);
        var time = 20;
        while(time > 0)
        {
            time --;
            print(time);
            countDownText.text = time.ToString();
            yield return new WaitForSeconds(1);
        }
        sceneLoader.LoadTitleMenu();
    }
    void GameOverMenu()
    {
        //restartButton.gameObject.SetActive(true);
        //countDownText.gameObject.SetActive(true);
        gameOverMenu.gameObject.SetActive(true);
    }

    IEnumerator TextEffect()
    {
        scoreKeeper.GetComponent<Transform>().localScale = new Vector3(0.9f, 0.9f, 1);
        yield return new WaitForSeconds(0.05f);
        scoreKeeper.GetComponent<Transform>().localScale = new Vector3(1f, 1f, 1f);
    }
}