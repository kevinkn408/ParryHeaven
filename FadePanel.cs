using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadePanel : MonoBehaviour
{
    GameManager gameManager;
  
    public void FadeOut()
    {
        GetComponent<Animator>().SetTrigger("LevelFadeOut");
    }

    public void AnimationResetRound()
    {
        FindObjectOfType<GameManager>().ResetRound();
    }
}
