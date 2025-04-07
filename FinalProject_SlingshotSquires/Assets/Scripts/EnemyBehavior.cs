using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource hitSound;
    private bool isDead = false;
    public AudioSource deathSound;
    public float health = 100f;
    public float enemySpeed = 1f;
    public GameObject GameHandler;
    private GameHandler gh;
    void Start()
    {
        gh = GameHandler.GetComponent<GameHandler>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isDead) transform.position -= Vector3.right * enemySpeed * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
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
    }

    private void Die()
    {
        isDead = true;
        deathSound.Play();
        FindObjectOfType<WaveManager>().EnemyKilled();
        Destroy(gameObject, deathSound.clip.length);
    }
}
