using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Button : MonoBehaviour
{
    EffectsManager effectsManager;
    // Start is called before the first frame update
    void Start()
    {
        effectsManager = FindObjectOfType<EffectsManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPress()
    {
        //GetComponent<Animator>().SetBool("IsPressed", true);
        //effectsManager.ArcadeButtonClick();

    }

    public void OnRelease()
    {
        //GetComponent<Animator>().SetBool("IsPressed", false);
    }
}
