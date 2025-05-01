using System.Collections.Generic;
using UnityEngine;

public class CropManager : MonoBehaviour
{
    public static CropManager Instance;
    
    public List<CropBehavior> activeCrops = new();

    void Awake() => Instance = this;

    public void RegisterCrop(CropBehavior crop)
    {
        if (!activeCrops.Contains(crop))
            activeCrops.Add(crop);
    }

    public void UnregisterCrop(CropBehavior crop)
    {
        if (activeCrops.Contains(crop))
            activeCrops.Remove(crop);
    }

    public CropBehavior GetNearestCrop(Vector3 fromPosition)
    {
        if (activeCrops == null || activeCrops.Count == 0) return null;

        CropBehavior closest = null;
        float shortest = Mathf.Infinity;

        foreach (var crop in activeCrops)
        {
            if (crop == null || !crop.IsAlive()) continue;

            float dist = Vector3.Distance(fromPosition, crop.transform.position);
            if (dist < shortest)
            {
                shortest = dist;
                closest = crop;
            }
        }

        return closest;
    }

    public void CleanupNullCrops()
    {
        activeCrops.RemoveAll(crop => crop == null);
    }


}
