using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelScript : MonoBehaviour
{
    CanvasGroup alphaChannel;
    float alphaChannelCurrent;
    // Start is called before the first frame update
    void Start()
    {
        alphaChannel = GetComponent<CanvasGroup>();
        alphaChannelCurrent = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        alphaChannel.alpha = Mathf.Lerp(alphaChannelCurrent, 0f, 3f);
    }
}
