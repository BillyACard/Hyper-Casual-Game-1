using System;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    private TextMeshProUGUI _textMeshProUGUI;

    private void Awake()
    {
        _textMeshProUGUI = GetComponent<TextMeshProUGUI>();
    }

    public void ChangeScore(int score)
    {
        _textMeshProUGUI.text = score.ToString();
    }

    private void OnEnable()
    {
        ScoreManager.Instance.OnScoreChange += ChangeScore;
    }

    private void OnDisable()
    {
        ScoreManager.Instance.OnScoreChange -= ChangeScore;    
    }
}
