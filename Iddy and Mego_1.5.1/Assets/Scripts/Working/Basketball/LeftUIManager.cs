using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeftUIManager : MonoBehaviour
{
    //[SerializeField] private Text scoreText;

    [SerializeField] private int score;

    Text scoreText;

    // Start is called before the first frame update
    void Start()
    {
        scoreText = GetComponent<Text>();
        scoreText.text = "Score: " + 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddScore() {
        score += 1;
        scoreText.text = "Score: " + score.ToString();
    }
}
