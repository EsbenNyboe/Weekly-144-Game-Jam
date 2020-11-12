using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    public TextMeshProUGUI wavesCount;

    public GameManager gameManager;

    public static WaveSpawner instance;

    public void StartLevel()
    {
        instance = this;
        waveIndex = 0;
        SpawnWave();
    }
    public void SpawnWave()
    {
        if (waveIndex < wave.Length)
        {
            wavesCount.text = wave.Length - waveIndex + " waves left";
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
        Transform eSpawnPos = wave[waveIndex].enemy[enemyIndex].enemySpawner.transform;
        Quaternion eSpawnRot = wave[waveIndex].enemy[enemyIndex].enemySpawner.transform.rotation;
        GameObject newEnemy = Instantiate(ePrefab, eSpawnPos.position, eSpawnRot);
        newEnemy.GetComponent<EnemyMovement>().initialDestination = eSpawnPos.GetChild(0).position;

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
        {
            AudioSystem.sb.waveCleared.PlayDefault();
            SpawnWave();
        }
    }
    public void ResetSpawner()
    {
        waveIndex = 0;
    }
}
