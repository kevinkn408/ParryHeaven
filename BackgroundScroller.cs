using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    Material material;
    [SerializeField] float horizontalSpeed = 1f;
    [SerializeField] float verticalSpeed = 1f;
    [SerializeField] bool rotate = false;
    [SerializeField] float spinRate = 1;
    // Start is called before the first frame update
    void Awake()
    {
        material = GetComponent<Renderer>().material;
        StartCoroutine(ScrollBackground());
    }

    // Update is called once per frame
    void Update()
    {
   
    }

    private IEnumerator ScrollBackground()
    {
        print("start");
        while (true)
        {
            material.mainTextureOffset += new Vector2(horizontalSpeed * Time.unscaledDeltaTime, verticalSpeed * Time.unscaledDeltaTime);
            if (rotate == true)
            {
                transform.Rotate(0, 0, spinRate * Time.unscaledDeltaTime);
            }
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }
}
