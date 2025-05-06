using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropSpawner : MonoBehaviour
{
    public GameObject starsPrefab;
    public GameObject growthPrefab;
    public WaveManager wave;

    public int gridWidth = 5;
    public int gridHeight = 4;
    public float cellWidth = 1.5f;
    public float cellHeight = 1.5f;

    public static List<GameObject> spawnedCrops = new();

    [SerializeField]
    private List<PlantPrefabEntry> plantPrefabsList;

    public Dictionary<string, GameObject> cropPrefabs = new();
    public AudioSource sellSound;
    

    [System.Serializable]
    public class PlantPrefabEntry
    {
        public string plantType;
        public GameObject prefab;
    }

    void Awake()
    {
        // Initialize crop prefab lookup
        foreach (var entry in plantPrefabsList)
        {
            if (!cropPrefabs.ContainsKey(entry.plantType))
            {
                cropPrefabs[entry.plantType] = entry.prefab;
            }
        }

        LoadCrops();
        StartCoroutine(DelayedEnemySpawn());
    }

    public void LoadCrops()
    {
        spawnedCrops.Clear();

        // Destroy previous crop instances
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        List<Crop> crops = GameHandler.cropInventory;

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                int index = x * gridHeight + y;
                if (index >= crops.Count)
                    return;

                Crop cropData = crops[index];

                if (!cropPrefabs.TryGetValue(cropData.cropName, out GameObject prefab))
                {
                    Debug.LogWarning($"No prefab found for crop type: {cropData.cropName}");
                    continue;
                }

                Vector3 localOffset = new Vector3(x * cellWidth, y * -cellHeight, 0);
                GameObject instance = Instantiate(prefab, transform);
                instance.transform.localPosition = localOffset;
                instance.transform.localRotation = Quaternion.identity;

                spawnedCrops.Add(instance);

                CropBehavior cb = instance.GetComponent<CropBehavior>();
                if (cb != null)
                    cb.Initialize(cropData);
            }
        }
    }

    public void ProgressCrops()
    {
        List<Crop> cropsToRemove = new();

        foreach (Crop crop in GameHandler.cropInventory)
        {

            crop.growthState++;
            if (growthPrefab != null)
                {
                    GameObject stars_0 = Instantiate(growthPrefab, transform.position, Quaternion.identity);
                    Destroy(stars_0, 1f);
                }

            if (crop.growthState >= crop.totalGrowthStates)
            {
                if (starsPrefab != null)
                {
                    GameObject stars = Instantiate(starsPrefab, transform.position, Quaternion.identity);
                    Destroy(stars, 1f);
                }
                GameHandler.coinCount += crop.salePrice;
                cropsToRemove.Add(crop);
                StartCoroutine(playSellSound());
            }
        }

        foreach (Crop crop in cropsToRemove)
        {
            GameHandler.cropInventory.Remove(crop);
        }
    }

    private IEnumerator playSellSound()
    {
        sellSound.Play();
        yield return new WaitForSeconds(sellSound.clip.length);
    }

    private IEnumerator DelayedEnemySpawn()
    {
        yield return new WaitForEndOfFrame(); // Wait so crops can register with CropManager
        CropManager.Instance.CleanupNullCrops();
        wave.SpawnNewEnemies();
        wave.CountTotalEnemies();
        wave.UpdateUI();
    }
}
