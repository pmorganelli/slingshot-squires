using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    public GameHandler.Crop thisCrop;

    public void Initialize(GameHandler.Crop crop)
    {
        thisCrop = crop;
    }

    public void cropDamage(int damage)
    {
        thisCrop.currHealth -= damage;
        // UPDATE HEALTH BAR UI HERE
        if (thisCrop.currHealth <= 0)
        {
            Debug.Log("Crop is dead! Removing from inventory");
            GameHandler.cropInventory.Remove(thisCrop);
            Destroy(gameObject);
        }
    }

}
