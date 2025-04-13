using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropSpawner : MonoBehaviour
{
    public int gridWidth = 5;
    public int gridHeight = 4;
    public float cellWidth = 1.5f;
    public float cellHeight = 1.5f;
    public Transform gridOrigin; // wherever we want the grid to start
    [SerializeField]
    private List<PlantPrefabEntry> plantPrefabsList;
    public Dictionary<string, GameObject> cropPrefabs = new();

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
    }
    // Start is called before the first frame update
    public void LoadCrops()
    {
        List<GameHandler.Crop> crops = GameHandler.cropInventory;

        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                int index = y * gridWidth + x;
                if (index >= GameHandler.cropInventory.Count)
                    return;

                GameHandler.Crop cropData = crops[index];
                GameObject prefab = cropPrefabs[cropData.cropName];

                Vector3 position = new Vector3(
                    x * cellWidth,
                    y * -cellHeight,
                    0);

                GameObject instance = Instantiate(prefab, position, Quaternion.identity);

                CropBehavior cb = instance.GetComponent<CropBehavior>();
                if (cb != null)
                    cb.Initialize(cropData);
            }
        }
    }
}
