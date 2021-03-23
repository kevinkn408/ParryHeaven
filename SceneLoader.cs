using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    //[SerializeField] AudioClip selectSound;
    //[SerializeField] GameObject fadeOut;
    Scene scene;
    string currentScene;
    [SerializeField] GameObject sceneTransitionFX;
    public delegate void SceneLoaderDelegate();
    public event SceneLoaderDelegate OnSceneChange;
    private static SceneLoader _instance;
    private static SceneLoader Instance { get { return _instance; } }


    private void OnLevelWasLoaded(int level)
    {
    }
    private void Awake()
    {
        sceneTransitionFX = GameObject.Find("Transition FX");

        //DontDestroyOnLoad(this.gameObject);
        //if(_instance != null && _instance != this)
        //{
        //    Destroy(this.gameObject);
        //}
        //else
        //{
        //    _instance = this;
        //}
    }

    private void Start()
    {
        //player.GetComponentInChildren<Character>().OnPlayerDeath += ReloadScene;
        currentScene = SceneManager.GetActiveScene().name;
    }

    public void ReloadScene()
    {
        StartCoroutine(ButtonSelect(currentScene));
    }

    public void LoadVersusScene()
    {
        // loadScreenPanel.gameObject.SetActive(true);
        StartCoroutine(ButtonSelect("Training"));
        //Enemy.allowInput = true;
        //Enemy.canBeHit = true;
        ResetValues();
    }

    public void Rematch()
    {
        SceneManager.LoadScene("Training");
        ResetValues();
    }

    private void ResetValues()
    {
        Time.timeScale = 1f;
    }

    public void LoadBonusScene()
    {
        ScoreManager.ResetValues();
        //Time.timeScale = 1f;
        SceneManager.LoadScene("Bonus Game");
        Debug.Log("fuck");
    }

    public void LoadSoloScene()
    {
        ScoreManager.ResetValues();
        //Time.timeScale = 1f;
        StartCoroutine(ButtonSelect("Solo Game"));
        Debug.Log("fuck");
    }

    public void LoadTitleMenu()
    {
        StartCoroutine(ButtonSelect("Title Menu Screen"));
    }

    public void LoadLoadScene()
    {
        StartCoroutine(ButtonSelect("Load Screen"));
    }

    IEnumerator ButtonSelect(string sceneName)
    {

        OnSceneChange?.Invoke();
        yield return new WaitForSecondsRealtime(0.25f);
        sceneTransitionFX.GetComponentInChildren<Animator>().SetTrigger("SceneChange");
        Time.timeScale = 1f;
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(sceneName);
    }
}
