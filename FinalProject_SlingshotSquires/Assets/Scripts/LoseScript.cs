using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using FMODUnity;
public class LoseScript : MonoBehaviour
{
    public TMP_Text stats;
    // Start is called before the first frame update
    private void Awake()
    {
        RuntimeManager.PlayOneShot("event:/SFX/Wave Lose");
        stats.text = "Wave Reached:" + GameHandler.levelCount +
                     "\nCoins Earned:" + GameHandler.totalMade +
                     "\nEnemies Killed:" + GameHandler.enemiesKilled +
                     "\nSeeds Planted:" + GameHandler.seedsPlanted;
    }
}
