using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public AudioSource hitSound;
    private bool isDead = false;
    public AudioSource deathSound;
    public float health = 100f;
    public float enemySpeed = 1f;
    public GameObject GameHandler;
    private GameHandler gh;

    private CurrencyManager currencyManager;
    public int valorCoinValue = 5;
    private Transform targetCrop;

    void Start()
    {
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
            ball.destroyBall();

            if (health <= 0)
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
            Destroy(other.gameObject); // Remove crop
            targetCrop = null;
            FindClosestCrop(); // Optional: move to next crop if any exist
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
