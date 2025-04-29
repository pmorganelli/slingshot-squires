using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class WaveManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    // Index into enemyPrefabs (scales with level)
    private int wormCt = 0;
    private static int maxWorms = 0;
    private static int slimeCt = 0;
    private int maxSlimes = 0;
    public Slider waveSlider;
    public TextMeshProUGUI waveCtrText;
    public TextMeshProUGUI waveText;
    private int totalEnemies;
    private int enemiesKilled;
    public CropSpawner spawner;

    public Transform SpawnPoint;
    public GameObject EnemyFab;

    void Start()
    {

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
        if (waveSlider.value == 1)
        {
            StartCoroutine(completeWave());
        }
    }

    private IEnumerator completeWave()
    {
        /* TODO: Give some indication wave is complete */
        spawner.ProgressCrops();
        spawner.LoadCrops(); // Load updated crops
        yield return new WaitForSeconds(1f); // Give player time to see their crops grow
        GameHandler.levelCount++;
        GameHandler.waveComplete = true; // go next wave.
    }

    public void SpawnNewEnemies()
    {
        waveCtrText.text = "Wave " + GameHandler.levelCount;
        wormCt = 0;
        slimeCt = 0;

        if (GameHandler.levelCount >= 4)
        {
            // 50/50 shot to increase max worms
            maxWorms += (int)Mathf.Round(Random.Range(0f, 2f));
            Debug.Log("MAXWORMS: " + maxWorms);
        }
        if (GameHandler.levelCount >= 8)
        {
            // 50/50 shot to increase max slimes
            maxSlimes += (int)Mathf.Round(Random.Range(0f, 2f));
            Debug.Log("MAXSLIMES: " + maxSlimes);
        }

        int enemy_ct = (int)Mathf.Round(1.6f * GameHandler.levelCount) + 3;

        for (int i = 0; i < enemy_ct; i++)
        {
            int enemyIdx = 0;
            float spawnRand = Random.Range(0f, 1f);
            if (spawnRand <= 0.4 && wormCt != maxWorms)
            {
                Debug.Log("WORM");
                // Spawn a worm
                enemyIdx = 1;
                wormCt++;
            }
            else if (spawnRand >= 0.75 && slimeCt != maxSlimes)
            {
                Debug.Log("SLIME");
                // Spawn a slime
                enemyIdx = 2;
                slimeCt++;
            }

            // Debug.Log("SPAWNING" + enemyPrefabs[enemyIdx].name);
            Vector3 spawnPt = new Vector3((SpawnPoint.position.x + 1), (SpawnPoint.position.y + (float)Random.Range(-5.5f, 5.5f)), 0);
            GameObject newEnemy = Instantiate(enemyPrefabs[enemyIdx], SpawnPoint);
            newEnemy.transform.position = spawnPt;
        }
    }
}
