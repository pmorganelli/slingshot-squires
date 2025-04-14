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
    private float damageTimer = 0f;
    private CurrencyManager currencyManager;
    public int valorCoinValue = 5;
    private Transform targetCrop;
    private CropBehavior targetCropStats;

    void Start()
    {
        currencyManager = FindObjectOfType<CurrencyManager>();
        FindClosestCrop();
    }

    void Update()
    {
        if (targetCropStats != null)
        {
            damageTimer += Time.deltaTime;
            if (damageTimer >= enemyAttackSpeed)
            {
                targetCropStats.cropDamage(enemyAttackDamage);
                damageTimer = 0f;
                if (targetCropStats.thisCrop.currHealth <= 0)
                {
                    targetCrop = null;
                    FindClosestCrop();
                }
            }
        }
    }
    void FixedUpdate()
    {
        if (!isDead && targetCrop != null)
        {
            float distance = Vector2.Distance(transform.position, targetCrop.position);
            if (distance > 0.1f)
            {
                Vector2 dir = (targetCrop.position - transform.position).normalized;
                transform.position += (Vector3)(dir * enemySpeed * Time.deltaTime);
            }
        }
        else
        {
            FindClosestCrop();
        }
    }

    private void FindClosestCrop()
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
                minDistance = distance;
                targetCrop = crop.transform;
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger Entered: " + other.gameObject.name);
        targetCropStats = other.GetComponent<CropBehavior>();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<CropBehavior>() == targetCropStats)
        {
            targetCrop = null;
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
        Destroy(gameObject, deathSound.clip.length);
    }
}
