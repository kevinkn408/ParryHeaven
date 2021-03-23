using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    Vector3 mousepos;
    //public GameObject mousePrefab;
    public AudioClip touchSound;
    [SerializeField] GameObject whiteFlash;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
         if (Input.GetMouseButtonDown(0))
        {
            mousepos = Input.mousePosition;
            mousepos.z = 10;
            //whiteFlash.gameObject.SetActive(true);
            var canvasGroup = whiteFlash.GetComponent<CanvasGroup>();
            StartCoroutine(WhiteScreenFlash(canvasGroup));
            mousepos = Camera.main.ScreenToWorldPoint(mousepos);
            //Instantiate(mousePrefab, mousepos, Quaternion.Euler(0, 0, Random.Range(120, 260)));
            AudioSource.PlayClipAtPoint(touchSound, Camera.main.transform.position, .5f);

        }
    }
    IEnumerator WhiteScreenFlash(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 1f;
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= 0.05f;
            yield return new WaitForSeconds(0.02f);
        }
    }
}
