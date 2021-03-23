using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    int ballCounter;
    //public Text ballCounterText;
    GameManager gameManager;
    [SerializeField] GameObject pauseMenu;
    Player[] player;

    Transform pauseMenuSwitch;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectsOfType<Player>();
        ballCounter = 0;
        //UpdateScoreText();
        pauseMenuSwitch = GetComponent<Transform>();
        gameManager = FindObjectOfType<GameManager>();
    }

    public void UpdateScore()
    {
        ballCounter = ballCounter + 1;
        //UpdateScoreText();
    }
    public void ResetScore()
    {
        ballCounter = 0;
        Debug.Log("score resetted");
    }

    //public void UpdateScoreText()
    //{
    //    ballCounterText.text = $"{ballCounter}";
    //}


    public void PauseToggle()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        if(pauseMenu.activeSelf == true)
        {
            foreach (Player pausePlayer in player)
            {
                pausePlayer.AllowInput = false;
            }
            gameManager.FreezeGame();
        }
        else
        {
            foreach (Player pausePlayer in player)
            {
                pausePlayer.AllowInput = true;
            }
            gameManager.UnfreezeGame();
        }
    }
}
