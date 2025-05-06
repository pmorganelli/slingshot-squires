using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using FMODUnity;

public class CropBehavior : MonoBehaviour
{
    [SerializeField] private SpriteRenderer cropRenderer;
    [SerializeField] private Sprite[] growthStages;

    public Crop thisCrop;
    public Slider healthBar;
    public AudioSource sellSound;

    public int attackers = 0;
    void Start()
    {
        CropManager.Instance.RegisterCrop(this);
    }

    void OnDestroy()
    {
        CropManager.Instance.UnregisterCrop(this);
    }

    public bool IsAlive() => thisCrop.currHealth > 0;

    public void Initialize(Crop crop)
    {
        thisCrop = crop;
        healthBar.value = (float)thisCrop.currHealth / (float)thisCrop.totalHealth;
        UpdateCropSprite();
        cropRenderer.color = Color.white;
    }

    public void UpdateCropSprite()
    {
        int stage = Mathf.Min(thisCrop.growthState, growthStages.Length - 1);
        cropRenderer.sprite = growthStages[stage];
    }

    public void AddStage()
    {
        thisCrop.growthState++;
        UpdateCropSprite();
    }

    public void cropDamage(int damage)
    {
        RuntimeManager.PlayOneShot("event:/SFX/Crop Hurt");
        thisCrop.currHealth -= damage;
        healthBar.value = (float)thisCrop.currHealth / (float)thisCrop.totalHealth;

        //darken crop visual
        Color color = cropRenderer.color;
        float darkenAmount = 0.1f;
        color.r = Mathf.Clamp01(color.r - darkenAmount);
        color.g = Mathf.Clamp01(color.g - darkenAmount);
        color.b = Mathf.Clamp01(color.b - darkenAmount);
        cropRenderer.color = color;

        // kill that thing if dead
        if (thisCrop.currHealth <= 0)
        {
            CropManager.Instance.UnregisterCrop(this);
            GameHandler.cropInventory.Remove(thisCrop);
            Destroy(gameObject);
        }
    }

    public Vector3 GetOffsetPosition()
    {
        float radius = 0.3f;
        float angle = attackers * 60f;
        Vector3 offset = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0) * radius;
        return transform.position + offset;
    }
}
