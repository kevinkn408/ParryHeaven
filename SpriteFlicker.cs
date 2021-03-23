using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFlicker : MonoBehaviour
{

    [SerializeField] float rate = 0.25f;
    Color alpha;

    // Start is called before the first frame update
    void Start()
    {
        alpha = GetComponent<SpriteRenderer>().material.color;
        StartCoroutine(Flicker());
    }

    private void Update()
    {
        //GetComponent<SpriteRenderer>().material.color = alpha;
    }

    IEnumerator Flicker()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        while (true)
        {
            alpha.a = 0.55f;
            spriteRenderer.material.color = alpha;
            yield return new WaitForSeconds(rate);
            alpha.a = 1f;
            spriteRenderer.material.color = alpha;
            yield return null;
        }
    }
}
