using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reset : Button
{
    LevelManager levelManager;
    // Start is called before the first frame update
  

    public void RestartLevelButton()
    {
        levelManager = levelManager.GetComponent<LevelManager>();
        levelManager.RestartLevel();
    }
}
