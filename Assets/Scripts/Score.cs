using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] public Text scoreText;

    private int totalScore;

    public int scoreMultiplayer;

    // Update is called once per frame
    void Update()
    {
        // scoreText.text = ((int)(player.position.z / 2)).ToString();

        totalScore += scoreMultiplayer;
        scoreText.text = totalScore.ToString();
    }
}
