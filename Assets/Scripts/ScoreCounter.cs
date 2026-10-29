using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ScoreCounter : MonoBehaviour
{
    [Header("Dynamic")]
    public int score = 0;
    private Text uiText;
    void Start()
    {
        uiText = GetComponent<Text>();
    }
    void Update()
    {
        uiText.text = score.ToString("#,0"); // This 0 is a zero!
    }
}
