using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSwitch : MonoBehaviour
{
    [SerializeField] bool isOn = false;
    // Start is called before the first frame update
    private void Start()
    {
        if (isOn)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    public void OnPress()
    {
        this.gameObject.SetActive(!this.gameObject.activeSelf);
    }
}
