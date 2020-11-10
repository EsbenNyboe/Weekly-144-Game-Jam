﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public Wave[] wave;

    [System.Serializable]
    public class Wave
    {
        public Enemy[] enemy;
    }

    [System.Serializable]
    public class Enemy
    {
        public GameObject prefab;
        public GameObject enemySpawner;
        public float spawnTiming;
    }

    int waveIndex;
    int enemyIndex;
    int enemiesAlive;

    public GameManager gameManager;

    public void SpawnWave()
    {
        if (waveIndex < wave.Length)
        {
            enemyIndex = 0;
            enemiesAlive = wave[waveIndex].enemy.Length;
            StartCoroutine(SpawnEnemy());
        }
        else
            Win();
    }
    IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(wave[waveIndex].enemy[enemyIndex].spawnTiming);

        GameObject ePrefab = wave[waveIndex].enemy[enemyIndex].prefab;
        Vector3 eSpawnPos = wave[waveIndex].enemy[enemyIndex].enemySpawner.transform.position;
        Quaternion eSpawnRot = wave[waveIndex].enemy[enemyIndex].enemySpawner.transform.rotation;
        Instantiate(ePrefab, eSpawnPos, eSpawnRot);

        enemyIndex++;
        if (enemyIndex < wave[waveIndex].enemy.Length)
            StartCoroutine(SpawnEnemy());
        else
        {
            waveIndex++;
        }
    }
    private void Win()
    {
        gameManager.Win();
        ResetSpawner();
    }

    // call these from elsewhere
    public void EnemyKilled()
    {
        enemiesAlive--;
        if (enemiesAlive < 1)
            SpawnWave();
    }
    public void ResetSpawner()
    {
        waveIndex = 0;
    }
}