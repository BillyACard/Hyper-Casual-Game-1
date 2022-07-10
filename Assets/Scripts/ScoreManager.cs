using System;
using System.Collections;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    private int _score = 0;
    public event Action<int> OnScoreChange = delegate {  };

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            return;
        }

        Destroy(this);
    }

    public void ResetScore()
    {
        _score = 0;
        OnScoreChange.Invoke(_score);
    }

    public int GetScore()
    {
        return _score;
    }

    public void HitWall()
    {
        ChangeScore(50);
    }

    public void StartCounting()
    {
        StartCoroutine(Counter());
    }

    public void StopCounting()
    {
        StopAllCoroutines();
    }

    private IEnumerator Counter()
    {
        WaitForSeconds second = new WaitForSeconds(1f);
        while (true)
        {
            ChangeScore(10);
            yield return second;
        }
    }

    private void ChangeScore(int scoreToAdd)
    {
        _score += scoreToAdd;
        if (_score < 0)
        {
            _score = 0;
        }
        OnScoreChange.Invoke(_score);
    }

    private void OnEnable()
    {
        PlayerController.OnHitWall += HitWall;
    }
}