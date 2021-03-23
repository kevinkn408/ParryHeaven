using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDeathLeftSide : MonoBehaviour
{
    ParticleSystem leftSideDeath;
    [SerializeField] AudioClip deathSFX;
    [SerializeField] AudioClip deathVoiceSFX;


    void Awake()
    {
        leftSideDeath = GetComponent<ParticleSystem>();
    }
    public void PlayDeathParticleLeft()
    {
        AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, 0.4f);
        AudioSource.PlayClipAtPoint(deathVoiceSFX, Camera.main.transform.position, .8f);
        leftSideDeath.Play();
    }
}
