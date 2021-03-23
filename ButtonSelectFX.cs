using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(AudioSource))]

public class ButtonSelectFX : MonoBehaviour
{
    Button button;
    CanvasGroup canvasGroup;
    [SerializeField] AudioSource buttonSFX;
    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPress()
    {
        buttonSFX.Play();

        StartCoroutine(Flicker());
    }

    IEnumerator Flicker()
    {
        while (true)
        {
            canvasGroup.alpha = 0.7f;
            yield return new WaitForSeconds(0.01f);
            canvasGroup.alpha = 0.5f;
            yield return null;
        }
    }
}
