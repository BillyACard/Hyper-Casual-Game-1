using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpikeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject spike;
    [SerializeField] private float spikeSpeed;
    [SerializeField] private Transform leftSpawn;
    [SerializeField] private Transform rightSpawn;
    [SerializeField] private int initialMinSpikes;
    [SerializeField] private int initialMaxSpikes;
    [SerializeField] private float minSpawnTimeAtStart;
    [SerializeField] private float maxSpawnTimeAtStart;
    [SerializeField] private float lowestMinSpawnTime;
    [SerializeField] private float lowestMaxSpawnTime;
    [SerializeField] private int numSpikesToReachLowest;
    [SerializeField] private int numSpikesForExtraSpike;
    [SerializeField] private int maxSpikes;
    
    public static SpikeSpawner Instance;
    private int _spikesSpawned;
    private float _minSpawnTime;
    private float _maxSpawnTime;
    private int _minSpikes;
    private int _maxSpikes;
    private bool _spawning;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            return;
        }

        Destroy(this);
    }

    public void StartSpawningSpikes()
    {
        if (_spawning) return;
        _spawning = true;
        _spikesSpawned = 0;
        _minSpawnTime = minSpawnTimeAtStart;
        _maxSpawnTime = maxSpawnTimeAtStart;
        _minSpikes = initialMinSpikes;
        _maxSpikes = initialMaxSpikes;
        StartCoroutine(SpawnLeftSide());
        StartCoroutine(SpawnRightSide());
    }

    public void StopSpawningSpikes()
    {
        if (!_spawning) return;
        _spawning = false;
        StopAllCoroutines();
    }

    private IEnumerator SpawnLeftSide()
    {
        float waitTime = Random.Range(_minSpawnTime, _maxSpawnTime);
        yield return new WaitForSeconds(waitTime);
        Spawn(GetRandomNumberOfSpikes(), leftSpawn.position);
        StartCoroutine(SpawnLeftSide());
    }

    private IEnumerator SpawnRightSide()
    {
        float waitTime = Random.Range(_minSpawnTime, _maxSpawnTime);
        yield return new WaitForSeconds(waitTime);
        Spawn(GetRandomNumberOfSpikes(), rightSpawn.position);
        StartCoroutine(SpawnRightSide());
    }

    private void DetermineSpikeSpawnSpeed()
    {
        _minSpawnTime = minSpawnTimeAtStart - Mathf.Clamp((float) _spikesSpawned / numSpikesToReachLowest, 0, 1) *
            (minSpawnTimeAtStart - lowestMinSpawnTime);
        _maxSpawnTime = maxSpawnTimeAtStart - Mathf.Clamp((float) _spikesSpawned / numSpikesToReachLowest, 0, 1) *
            (maxSpawnTimeAtStart - lowestMaxSpawnTime);
    }

    private void CheckForExtraSpikes()
    {
        if (_maxSpikes >= maxSpikes) return;
        if (_spikesSpawned / numSpikesForExtraSpike > 0)
        {
            _maxSpikes++;
            _spikesSpawned -= numSpikesForExtraSpike;
        }
    }
    
    private int GetRandomNumberOfSpikes()
    {
        return Random.Range(_minSpikes, _maxSpikes + 1);
    }

    private void Spawn(int num, Vector3 position)
    {
        for (int i = 0; i < num; i++)
        {
            GameObject spawnedSpike = Instantiate(spike, position + new Vector3(0f, 0.5f * i, 0f), Quaternion.identity);
            spawnedSpike.transform.DOMoveY(-9f + 0.5f * i, 120f/spikeSpeed).SetEase(Ease.Linear).OnComplete(() => Destroy(spawnedSpike));
        }
        _spikesSpawned++;
        CheckForExtraSpikes();
        DetermineSpikeSpawnSpeed();
    }
}
