using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    private Sean _sean;
    public BallSet[] _UltraSet;
    [SerializeField] int secondsToStart = 0;

    //void Awake()
    //{
    //}

    void Start()
    {
        _sean = UnityEngine.GameObject.Find("Sean").GetComponent<Sean>();  
      StartCoroutine(StartLevel(secondsToStart));
    }

    IEnumerator StartLevel(int startDelay)
    {

        yield return new WaitForSeconds(startDelay);
        for (int i = 0; i <= _UltraSet.Length; i++)
        {
          yield return new WaitForSeconds(_UltraSet[i].delayUntilJump);
          _sean.JumpShoot(_UltraSet[i]);
        }
        //add a button after this loop is done to restart the game
    }

    void Update()
    {
      if (Input.GetKeyDown(KeyCode.R))
      {
        RestartLevel();
      }
        
    }

    public void RestartLevel()
    {
      StartCoroutine(StartLevel(0));
    }
}
