using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroLine : MonoBehaviour
{
    public AudioClip fightReady;
    // Start is called before the first frame update
    void StartSound()
    {
        AudioSource.PlayClipAtPoint(fightReady, Camera.main.transform.position, 0.5f);
        Destroy(this.gameObject, this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);

    }
}
