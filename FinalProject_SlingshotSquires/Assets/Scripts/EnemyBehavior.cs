using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBehavior : MonoBehaviour
{
    public Slider healthBar;
    public AudioSource hitSound;
    private bool isDead = false;
    public AudioSource deathSound;

    public float totalHealth = 100f;
    public float currHealth = 100f;
    public float enemySpeed = 1f;
    public float enemyAttackSpeed = 1f;
    public int enemyAttackDamage = 10;
    public int valorCoinValue = 5;

    private float damageTimer = 0f;
    private CurrencyManager currencyManager;

    private Transform targetCrop;
    private CropBehavior targetCropStats;

    // === Slot System ===
    private Transform slotTarget;
    private int slotIndex = -1;
    private CropSlotManager cropSlotManager;

    void Start()
    {
        currencyManager = FindObjectOfType<CurrencyManager>();
        FindClosestCrop();
    }

    void Update()
    {
        if (!GameHandler.waveStarted) return;
        if (targetCropStats != null && slotTarget != null)
        {
            float distance = Vector2.Distance(transform.position, slotTarget.position);

            // Only attack if fully arrived
            if (distance <= 0.02f)
            {
                damageTimer += Time.deltaTime;

                if (damageTimer >= enemyAttackSpeed)
                {
                    damageTimer = 0f;

                    // Double-check that thisCrop is still valid before applying damage
                    if (targetCropStats != null && targetCropStats.thisCrop != null)
                    {
                        targetCropStats.cropDamage(enemyAttackDamage);
                    }

                    // If crop is dead or missing, move on
                    if (targetCropStats == null || targetCropStats.thisCrop == null || targetCropStats.thisCrop.currHealth <= 0)
                    {
                        // Unclaim slot
                        if (cropSlotManager != null && slotIndex != -1)
                        {
                            cropSlotManager.ReleaseSlot(slotIndex);
                        }

                        // Reset everything
                        targetCrop = null;
                        targetCropStats = null;
                        slotTarget = null;
                        cropSlotManager = null;
                        slotIndex = -1;

                        FindClosestCrop();
                    }
                }
            }
        }
    }




    void FixedUpdate()
    {
        if (!GameHandler.waveStarted) return;
        if (!isDead && slotTarget != null)
        {
            float distance = Vector2.Distance(transform.position, slotTarget.position);
            if (distance > 0.02f)
            {
                Debug.DrawLine(transform.position, slotTarget.position, Color.red);
                Vector2 dir = (slotTarget.position - transform.position).normalized;
                transform.position += (Vector3)(dir * enemySpeed * Time.deltaTime);
            }
        }
        else if (!isDead)
        {
            FindClosestCrop();
        }
    }

    public void FindClosestCrop()
    {
        GameObject[] crops = GameObject.FindGameObjectsWithTag("Crop");
        float minDistance = Mathf.Infinity;

        if (crops.Length == 0)
        {
            GameHandler.lost = true;
            return;
        }

        foreach (GameObject crop in crops)
        {
            float distance = Vector2.Distance(transform.position, crop.transform.position);
            if (distance < minDistance)
            {
                CropSlotManager slotManager = crop.GetComponent<CropSlotManager>();
                if (slotManager == null) continue;

                Transform availableSlot = slotManager.ClaimSlot(out int claimedIndex);
                if (availableSlot != null)
                {
                    minDistance = distance;
                    targetCrop = crop.transform;
                    targetCropStats = crop.GetComponent<CropBehavior>();
                    cropSlotManager = slotManager;
                    slotTarget = availableSlot;
                    slotIndex = claimedIndex;
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (targetCropStats == null && other.GetComponent<CropBehavior>() != null)
        {
            targetCropStats = other.GetComponent<CropBehavior>();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<CropBehavior>() == targetCropStats)
        {
            targetCrop = null;
            targetCropStats = null;
            damageTimer = 0f;
            FindClosestCrop();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (isDead) return;

        if (other.gameObject.CompareTag("Projectile"))
        {
            BallMovement ball = other.gameObject.GetComponent<BallMovement>();
            currHealth -= GameHandler.ballStats[ball.ballType].damage;
            healthBar.value = (currHealth / totalHealth);
            ball.destroyBall();

            if (currHealth <= 0)
            {
                Die();
            }
            else
            {
                hitSound.Play();
            }
        }
    }

    private void Die()
    {
        isDead = true;
        deathSound.Play();
        FindObjectOfType<WaveManager>().EnemyKilled();

        if (currencyManager != null)
        {
            currencyManager.AddValorCoins(valorCoinValue);
        }

        ReleaseCropSlot();
        Destroy(gameObject, deathSound.clip.length);
    }


    private void OnDestroy()
    {
        ReleaseCropSlot();
    }


    private void ReleaseCropSlot()
    {
        if (cropSlotManager != null && slotIndex != -1)
        {
            cropSlotManager.ReleaseSlot(slotIndex);
            cropSlotManager = null;
            slotIndex = -1;
            slotTarget = null;
        }
    }

    private void OnDisable()
    {
        ReleaseCropSlot();
    }


}
