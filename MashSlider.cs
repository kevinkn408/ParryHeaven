using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MashSlider : MonoBehaviour
{
    Slider slider;
    public Image image;
    Color minMeterColor = Color.yellow;
    Color maxMeterColor = Color.green;
    float val;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();

    }

    // Update is called once per frame
    void Update()
    {
        UpdateColorValue();
    }

    private void UpdateColorValue()
    {
        image.color = Color.Lerp(minMeterColor, maxMeterColor, slider.value/5);
    }
}
