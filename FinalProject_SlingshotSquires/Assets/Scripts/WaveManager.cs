using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class WaveManager : MonoBehaviour
{

    public GameObject defaultBall;
    public GameObject LoseTxt;
    [Header("Enemy Setup")]
    public GameObject[] enemyPrefabs;

    private int wormCt = 0;
    private static int maxWorms = 0;
    private static int slimeCt = 0;
    private int maxSlimes = 0;

    [Header("UI References")]
    public Slider waveSlider;
    public TextMeshProUGUI waveCtrText;
    public TextMeshProUGUI waveText;

    private int totalEnemies;
    private int enemiesKilled;

    [Header("Spawning")]
    public CropSpawner spawner;
    public Transform SpawnPoint;
    public GameObject EnemyFab;

    private void Update()
    {
        if (GameHandler.lost)
        {
            GameHandler.lost = false;
            StartCoroutine(LoseGame());
        }
    }

    private IEnumerator LoseGame()
    {
        LoseTxt.SetActive(true);
        yield return new WaitForSeconds(1.75f);
        LoseTxt.SetActive(false);
        SceneManager.LoadScene("waveLose");

    }

    public void CountTotalEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        totalEnemies = enemies.Length;
    }

    public void EnemyKilled()
    {
        enemiesKilled++;
        UpdateUI();
    }

    public void UpdateUI()
    {
        float progress = totalEnemies > 0 ? (float)enemiesKilled / totalEnemies : 0;
        waveSlider.value = progress;
        waveText.text = $"{enemiesKilled}/{totalEnemies} Enemies Killed";

        if (progress >= 1f)
        {
            StartCoroutine(completeWave());
        }
    }

    private IEnumerator completeWave()
    {
        Sling.ChangeBall(defaultBall);
        CropManager.Instance.CleanupNullCrops();
        spawner.ProgressCrops();
        //spawner.LoadCrops();

        yield return new WaitForSeconds(1f);

        GameHandler.levelCount++;
        GameHandler.waveComplete = true;
    }

    public void SpawnNewEnemies()
    {
        waveCtrText.text = "Wave " + GameHandler.levelCount;

        wormCt = 0;
        slimeCt = 0;

        if (GameHandler.levelCount >= 2)
        {
            maxWorms += Random.Range(0, 2); // 0 or 1
            Debug.Log("MAXWORMS: " + maxWorms);
        }

        if (GameHandler.levelCount >= 4)
        {
            maxSlimes += Random.Range(0, 2);
            Debug.Log("MAXSLIMES: " + maxSlimes);
        }

        int enemy_ct = Mathf.RoundToInt(1.6f * GameHandler.levelCount) + 3;

        for (int i = 0; i < enemy_ct; i++)
        {
            int enemyIdx = 0;
            float spawnRand = Random.Range(0f, 1f);

            if (spawnRand <= 0.4f && wormCt < maxWorms)
            {
                enemyIdx = 1;
                wormCt++;
                Debug.Log("WORM");
            }
            else if (spawnRand >= 0.75f && slimeCt < maxSlimes)
            {
                enemyIdx = 2;
                slimeCt++;
                Debug.Log("SLIME");
            }

            Vector3 spawnPt = new Vector3(
                SpawnPoint.position.x + 1,
                SpawnPoint.position.y + Random.Range(-5.5f, 5.5f),
                0
            );

            GameObject newEnemy = Instantiate(enemyPrefabs[enemyIdx], spawnPt, Quaternion.identity);
        }
    }
}
