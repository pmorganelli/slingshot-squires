using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CropBehavior : MonoBehaviour
{
    [SerializeField] private SpriteRenderer cropRenderer;
    [SerializeField] private Sprite[] growthStages;
    // Start is called before the first frame update
    public Crop thisCrop;
    public Slider healthBar;

    public void Initialize(Crop crop)
    {
        thisCrop = crop;
        healthBar.value = thisCrop.currHealth / thisCrop.totalHealth;
        UpdateCropSprite();
    }

    private void UpdateCropSprite()
    {
        if (thisCrop.growthState < growthStages.Length)
        {
            cropRenderer.sprite = growthStages[thisCrop.growthState];
        }
        else
        {
            Debug.LogWarning("Growth state exceeds available sprites. Using last sprite.");
            cropRenderer.sprite = growthStages[growthStages.Length - 1];
        }
    }
    public void cropDamage(int damage)
    {
        thisCrop.currHealth -= damage;
        healthBar.value = thisCrop.currHealth / thisCrop.totalHealth;
        // UPDATE HEALTH BAR UI HERE
        if (thisCrop.currHealth <= 0)
        {
            Debug.Log("Crop is dead! Removing from inventory");
            GameHandler.cropInventory.Remove(thisCrop);
            Destroy(gameObject);
        }
    }

}
