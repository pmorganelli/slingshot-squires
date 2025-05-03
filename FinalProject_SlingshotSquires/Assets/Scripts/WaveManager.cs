using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class WaveManager : MonoBehaviour
{
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

    [Header("Slingshot Setup")]
    public GameObject sling;                // Drag the Sling GameObject in Inspector
    public GameObject ballPrefab;          // Drag the Ball prefab in Inspector

    void Start()
    {
        AssignSlingReferences();
    }

    private void AssignSlingReferences()
    {
        if (sling == null)
        {
            sling = GameObject.FindWithTag("Sling"); // Optional fallback if not assigned
        }

        if (sling != null && ballPrefab != null)
        {
            Sling slingScript = sling.GetComponent<Sling>();
            if (slingScript != null)
            {
                slingScript.nextBall = ballPrefab;
                slingScript.fallbackBallPrefab = ballPrefab;
                Debug.Log("[WaveManager] Assigned ball prefab to Sling");
            }
            else
            {
                Debug.LogWarning("[WaveManager] Sling GameObject has no Sling script attached");
            }
        }
        else
        {
            Debug.LogWarning("[WaveManager] sling or ballPrefab not assigned. Ball reload may fail.");
        }
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
        CropManager.Instance.CleanupNullCrops();
        spawner.ProgressCrops();
        spawner.LoadCrops();

        yield return new WaitForSeconds(1f);

        GameHandler.levelCount++;
        GameHandler.waveComplete = true;

        // Ensure sling is still linked after scene reload
        AssignSlingReferences();

        if (sling != null)
        {
            Sling slingScript = sling.GetComponent<Sling>();
            if (slingScript != null && slingScript.nextBall != null)
            {
                Debug.Log("[WaveManager] Reloading ball after wave complete");
                slingScript.reload();
            }
            else
            {
                Debug.LogWarning("[WaveManager] Cannot reload ball – Sling script or prefab missing");
            }
        }
    }

    public void SpawnNewEnemies()
    {
        waveCtrText.text = "Wave " + GameHandler.levelCount;

        wormCt = 0;
        slimeCt = 0;

        if (GameHandler.levelCount >= 4)
        {
            maxWorms += Random.Range(0, 2); // 0 or 1
            Debug.Log("MAXWORMS: " + maxWorms);
        }

        if (GameHandler.levelCount >= 8)
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
