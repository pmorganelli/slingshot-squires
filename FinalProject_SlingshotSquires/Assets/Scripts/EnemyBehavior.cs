using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required for UI elements

public class EnemyBehavior : MonoBehaviour
{
    public AudioSource hitSound;
    public AudioSource deathSound;

    private bool isDead = false;

    public float maxHealth = 100f;
    public float minHealth = 0f;

    public float enemySpeed = 1f;
    public GameObject GameHandler;
    private GameHandler gh;

    private CurrencyManager currencyManager;
    public int valorCoinValue = 5;
    private Transform targetCrop;

    // (Optional) UI Slider to show health bar
    public EnemyHealthBar healthBar;

    void Start()
    {
        health = Mathf.Clamp(health, minHealth, maxHealth); // Clamp at start
        healthBar = GetComponentInChildren<EnemyHealthBar>();
        healthBar.UpdateHealthBar(health, maxHealth);

        gh = GameHandler.GetComponent<GameHandler>();
        currencyManager = FindObjectOfType<CurrencyManager>();
        FindClosestCrop();
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
    }

    private void FindClosestCrop()
    {
        GameObject[] crops = GameObject.FindGameObjectsWithTag("Crop");
        float minDistance = Mathf.Infinity;

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

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (isDead) return;

        if (other.gameObject.CompareTag("Projectile"))
        {
            BallMovement ball = other.gameObject.GetComponent<BallMovement>();
            health -= gh.ballStats[ball.ballType].damage;
            health = Mathf.Clamp(health, minHealth, maxHealth); // Clamp after taking damage
            healthBar.UpdateHealthBar(health, maxHealth);

            ball.destroyBall();

            if (health <= minHealth)
            {
                Die();
            }
            else
            {
                hitSound.Play();
            }
        }

        if (other.gameObject.CompareTag("Crop"))
        {
            Destroy(other.gameObject);
            targetCrop = null;
            FindClosestCrop();
        }
    }

    // private void UpdateHealthBar()
    // {
    //     if (healthBar != null)
    //     {
    //         healthBar.value = health / maxHealth;
    //     }
    // }

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
