using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class WaveManager : MonoBehaviour
{
    public Slider waveSlider;
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
        int enemy_ct = (2 * GameHandler.levelCount) + 3;

        for (int i = 0; i < enemy_ct; i++)
        {
            Debug.Log("SPAWNING" + i);
            Vector3 spawnPt = new Vector3(SpawnPoint.position.x, (SpawnPoint.position.y + (float)Random.Range(-5.5f, 5.5f)), 0);
            GameObject newEnemy = Instantiate(EnemyFab, SpawnPoint);
            newEnemy.transform.position = spawnPt;
        }
    }
}
