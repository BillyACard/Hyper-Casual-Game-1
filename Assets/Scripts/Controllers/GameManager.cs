using System;
using Unity.Mathematics;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform playerSpawnLocation;
    public static GameManager Instance;
    private bool _playing;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            return;
        }
        Destroy(this);
    }

    public void StartGame()
    {
        if (_playing) return;
        _playing = true;
        Instantiate(playerPrefab, playerSpawnLocation.position, quaternion.identity);
        SpikeSpawner.Instance.StartSpawningSpikes();
        ScoreManager.Instance.StartCounting();
    }

    public void GameOver()
    {
        if (!_playing) return;
        _playing = false;
        SpikeSpawner.Instance.StopSpawningSpikes();
        ScoreManager.Instance.StopCounting();
        MenuController.Instance.OnGameOver();
    }

    private void OnEnable()
    {
        PlayerController.OnDeath += GameOver;
    }

    private void OnDisable()
    {
        PlayerController.OnDeath -= GameOver;
    }
}
