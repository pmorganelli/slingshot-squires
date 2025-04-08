using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaveManager : MonoBehaviour
{
    public Slider waveSlider;
    public TextMeshProUGUI waveText;
    public GameObject GameHandler;
    private GameHandler gh;

    private int totalEnemies;
    private int enemiesKilled;

    void Start()
    {
        gh = GameHandler.GetComponent<GameHandler>();
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
            gh.waveComplete = true;
        }
    }
}
