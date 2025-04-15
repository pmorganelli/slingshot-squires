using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CropBehavior : MonoBehaviour
{
    [SerializeField] private SpriteRenderer cropRenderer;
    [SerializeField] private Sprite[] growthStages;

    public Crop thisCrop;
    public Slider healthBar;
    public AudioSource sellSound;

    public void Initialize(Crop crop)
    {
        thisCrop = crop;
        healthBar.value = (float)thisCrop.currHealth / (float)thisCrop.totalHealth;
        UpdateCropSprite();
        cropRenderer.color = Color.white;
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

        float darkenAmount = 0.1f;
        Color color = cropRenderer.color;
        color.r = Mathf.Clamp01(color.r - darkenAmount);
        color.g = Mathf.Clamp01(color.g - darkenAmount);
        color.b = Mathf.Clamp01(color.b - darkenAmount);
        cropRenderer.color = color;

        if (thisCrop.currHealth <= 0)
        {
            GameHandler.cropInventory.Remove(thisCrop);
            Destroy(gameObject);
        }
    }
}
