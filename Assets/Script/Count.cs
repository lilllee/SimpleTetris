using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Count : MonoBehaviour
{
    public Text scoreText;
    public Image gameOver;
    void Update()
    {
        scoreText.text = "Score: " + Grid.count;
    }
}
