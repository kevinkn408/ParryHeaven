using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationDestroyer : MonoBehaviour
{
    // Start is called before the first frame update
    public void SelfDestruct()
    {
        Destroy(gameObject);
    }
}
