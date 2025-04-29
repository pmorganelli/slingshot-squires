using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class CropSpawner : MonoBehaviour
{
    public WaveManager wave;
    public int gridWidth = 5;
    public int gridHeight = 4;
    public float cellWidth = 1.5f;
    public float cellHeight = 1.5f;
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
        foreach (var entry in plantPrefabsList)
        {
            cropPrefabs[entry.plantType] = entry.prefab;
        }
        LoadCrops();
        wave.SpawnNewEnemies();
        wave.CountTotalEnemies();
        wave.UpdateUI();
    }
    public void LoadCrops()
    {

        // Clear existing crops in the grid
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
                if (index >= GameHandler.cropInventory.Count)
                    return;

                Crop cropData = crops[index];
                // Debug.Log("Spawning crop: " + cropData.cropName + " at index: " + index);
                GameObject prefab = cropPrefabs[cropData.cropName];

                Vector3 localOffset = new Vector3(x * cellWidth, y * -cellHeight, 0);

                GameObject instance = Instantiate(prefab, gameObject.transform);
                instance.transform.localPosition = localOffset;
                instance.transform.localRotation = Quaternion.identity;

                CropBehavior cb = instance.GetComponent<CropBehavior>();
                if (cb != null)
                    cb.Initialize(cropData);
            }
        }
    }

    public void ProgressCrops()
    {
        List<Crop> cropsToRemove = new List<Crop>();
        int ctr = 0;
        foreach (Crop crop in GameHandler.cropInventory)
        {
            ctr++;
            crop.growthState += 1;

            if (crop.growthState >= crop.totalGrowthStates)
            {
                Debug.Log("SOLD " + ctr);
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
}
