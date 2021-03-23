using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitToLoadNextScene());
    }

    // Update is called once per frame


    IEnumerator WaitToLoadNextScene()
    {
        yield return new WaitForSecondsRealtime(2);
        SceneManager.LoadScene("Versus P2 Beta");
    }
}
