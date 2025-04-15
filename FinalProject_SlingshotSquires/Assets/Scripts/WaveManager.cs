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

    void Start()
    {
        CountTotalEnemies();
        UpdateUI();
    }

    void CountTotalEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        totalEnemies = enemies.Length;
    }

    public void EnemyKilled()
    {
        enemiesKilled++;
        UpdateUI();
    }

    void UpdateUI()
    {
        float progress = totalEnemies > 0 ? (float)enemiesKilled / totalEnemies : 0;
        waveSlider.value = progress;
        waveText.text = $"{enemiesKilled} / {totalEnemies} Enemies Killed";
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
        yield return new WaitForSeconds(5f); // Give player time to see their crops grow
        GameHandler.waveCount++;
        GameHandler.waveComplete = true; // go next wave.
    }
}
