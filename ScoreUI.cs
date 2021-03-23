using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] Character character;
    Text scoreText;
    int score;
    // Start is called before the first frame update
    private void Start()
    {
        scoreText = GetComponent<Text>();
    }

    private void Update()
    {
    }

    public void UpdateScoreUI()
    {
        score = character.parryScore;
        scoreText.text = score.ToString();
    }
}
