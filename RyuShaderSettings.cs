using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RyuShaderSettings : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Renderer renderer = GetComponent<Renderer>();
        // if (renderer == null){
        //     Debug.Log("Renderer is empty");
        // }
        GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.TwoSided;
        GetComponent<Renderer>().receiveShadows = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
