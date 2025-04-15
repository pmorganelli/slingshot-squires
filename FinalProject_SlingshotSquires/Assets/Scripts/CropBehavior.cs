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
        // GameHandler.existingCropObjects.Add(gameObject.GetComponent<CropBehavior>());
        thisCrop = crop;
        healthBar.value = (float)thisCrop.currHealth / (float)thisCrop.totalHealth;
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
            cropRenderer.sprite = growthStages[growthStages.Length - 1];
        }
    }

    public void AddStage()
    {
        thisCrop.growthState++;
        UpdateCropSprite();
    }
    public void cropDamage(int damage)
    {
        thisCrop.currHealth -= damage;
        healthBar.value = (float)thisCrop.currHealth / (float)thisCrop.totalHealth;
        if (thisCrop.currHealth <= 0)
        {
            Debug.Log("Crop is dead! Removing from inventory");
            GameHandler.cropInventory.Remove(thisCrop);
            Destroy(gameObject);
        }
    }

}
