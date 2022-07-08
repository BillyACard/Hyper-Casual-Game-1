using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpikeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject spike;
    [SerializeField] private float spikeSpeed;
    [SerializeField] private Transform leftSpawn;
    [SerializeField] private Transform rightSpawn;
    public static SpikeSpawner Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            return;
        }

        Destroy(this);
    }

    private void Start()
    {
        StartCoroutine(SpawnLeftSide());
        StartCoroutine(SpawnRightSide());
    }

    private IEnumerator SpawnLeftSide()
    {
        float waitTime = Random.Range(1.5f, 3f);
        yield return new WaitForSeconds(waitTime);
        Spawn(3, leftSpawn.position);
        StartCoroutine(SpawnLeftSide());
    }

    private IEnumerator SpawnRightSide()
    {
        float waitTime = Random.Range(1.5f, 3f);
        yield return new WaitForSeconds(waitTime);
        Spawn(3, rightSpawn.position);
        StartCoroutine(SpawnRightSide());
    }

    private void Spawn(int num, Vector3 position)
    {
        for (int i = 0; i < num; i++)
        {
            Spike spawnedSpike = Instantiate(spike, position + new Vector3(0f, 0.5f * i, 0f), Quaternion.identity).GetComponent<Spike>();
            spawnedSpike.Init(spikeSpeed);
        }
    }
}
