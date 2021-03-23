using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultsScreen : MonoBehaviour
{
    bool playerAccept;
    bool enemyAccept;
    SceneLoader sceneLoader;
    [SerializeField] GameObject confirmPlayer;
    [SerializeField] GameObject confirmEnemy;

    // Start is called before the first frame update
    private void Awake()
    {
        playerAccept = false;
        enemyAccept = false;
    }

    void Start()
    {
        sceneLoader = FindObjectOfType<SceneLoader>();
    }

    public void PlayerRematch()
    {
        Debug.Log("player accept");

        playerAccept = true;
        confirmPlayer.gameObject.SetActive(true);
        Rematch();
    }

    public void EnemyRematch()
    {
        Debug.Log("enemy accept");

        enemyAccept = true;
        confirmEnemy.gameObject.SetActive(true);
        Rematch();
    }

    private void Rematch()
    {
        if(playerAccept && enemyAccept)
        {
            StartCoroutine(DelayBeforeRestart());
        }
        else
        {
            return;
        }
    }

    IEnumerator DelayBeforeRestart()
    {
        yield return new WaitForSeconds(.25f);
        sceneLoader.Rematch();

    }
}
