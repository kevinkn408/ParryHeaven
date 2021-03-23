using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOut : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Camera mainCamera;
    CanvasGroup canvasGroup;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        mainCamera.GetComponent<AudioSource>().volume -= 0.02f;
        canvasGroup.alpha += 0.10f;
    }
}
