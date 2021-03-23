using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDeathRightSide : MonoBehaviour
{
    ParticleSystem rightSideDeath;
    [SerializeField] AudioClip deathSFX;
    [SerializeField] AudioClip deathVoiceSFX;


    void Start()
    {
        rightSideDeath = GetComponent<ParticleSystem>();
    }
    public void PlayDeathParticleRight()
    {
        AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, 0.4f);
        AudioSource.PlayClipAtPoint(deathVoiceSFX, Camera.main.transform.position, .8f);
        rightSideDeath.Play();
    }
}

