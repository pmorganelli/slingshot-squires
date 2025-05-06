using UnityEngine;
using FMODUnity;
using UnityEngine.UI;

public class EnemyBehavior : MonoBehaviour
{
    public float totalHealth = 100f;
    public float currHealth = 100f;
    public float enemySpeed = 1f;
    public float attackRate = 1f;
    public int enemyAttackDamage = 25;
    public int valorCoinValue = 5;

    public Slider healthBar;
    public AudioSource hitSound;
    public AudioSource deathSound;
    public GameObject poofPrefab;

    private float attackCooldown = 0f;
    private bool isDead = false;

    private CropBehavior targetCrop;
    private Vector3 attackPosition;
    private CurrencyManager currencyManager;

    void Start()
    {
        currencyManager = FindObjectOfType<CurrencyManager>();
        FindNewCrop();
    }

    void Update()
    {
        if (isDead || !GameHandler.waveStarted) return;

        if (targetCrop == null || !targetCrop.IsAlive())
        {
            FindNewCrop();
            return;
        }

        float dist = Vector2.Distance(transform.position, attackPosition);
        if (dist > 0.05f)
        {
            Vector2 dir = (attackPosition - transform.position).normalized;
            transform.position += (Vector3)(dir * enemySpeed * Time.deltaTime);
        }
        else
        {
            attackCooldown -= Time.deltaTime;
            if (attackCooldown <= 0f)
            {
                targetCrop.cropDamage(enemyAttackDamage);
                attackCooldown = attackRate;

                if (!targetCrop.IsAlive())
                {
                    targetCrop.attackers--;
                    targetCrop = null;
                    FindNewCrop();
                }
            }
        }
    }

    void FindNewCrop()
    {
        CropBehavior best = null;
        float shortest = Mathf.Infinity;

        foreach (var crop in CropManager.Instance.activeCrops)
        {
            if (crop == null || !crop.IsAlive()) continue;
            float dist = Vector2.Distance(transform.position, crop.transform.position);
            if (dist < shortest)
            {
                shortest = dist;
                best = crop;
            }
        }

        if (best != null)
        {
            targetCrop = best;
            targetCrop.attackers++;
            attackPosition = targetCrop.GetOffsetPosition();
        }
        else
        {
            GameHandler.lost = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        if (collision.gameObject.CompareTag("Projectile"))
        {
            BallMovement ball = collision.gameObject.GetComponent<BallMovement>();
            if (ball == null) return;

            if (!GameHandler.ballStats.TryGetValue(ball.ballType, out GameHandler.ballStat stat))
                return;

            // Debug.Log($"[Enemy] Hit by {ball.ballType}, Damage: {stat.damage}");

            currHealth -= stat.damage;
            healthBar.value = currHealth / totalHealth;

            ball.destroyBall();

            if (currHealth <= 0f)
            {
                Die();
            }
            else if (hitSound != null)
            {
                RuntimeManager.PlayOneShot("event:/SFX/Worm Hurt");
            }
        }
    }




    void Die()
    {
        if (isDead) return; // Prevent double calls

        isDead = true;

        if (deathSound != null)
            RuntimeManager.PlayOneShot("event:/SFX/Bug Death");

        if (poofPrefab != null)
        {
            GameObject poof = Instantiate(poofPrefab, transform.position, Quaternion.identity);
            Destroy(poof, 1f);
        }

        if (targetCrop != null)
        {
            targetCrop.attackers--;
            targetCrop = null;
        }

        if (currencyManager != null)
            currencyManager.AddValorCoins(valorCoinValue);

        WaveManager wave = FindObjectOfType<WaveManager>();
        if (wave != null)
            wave.EnemyKilled();

        Destroy(gameObject, deathSound != null ? deathSound.clip.length : 0.5f);
    }

    void OnDestroy()
    {
        if (!isDead && targetCrop != null)
            targetCrop.attackers--;
    }


    void OnDisable()
    {
        if (targetCrop != null)
            targetCrop.attackers--;
    }
}
