using UnityEngine;
using System.Collections;
using FMODUnity;
public class Sling : MonoBehaviour
{
    [Header("Prefab Settings")]
    public GameObject nextBall;
    public GameObject defaultBallPrefab;
    public GameObject goldBallPrefab;
    public GameObject diamondBallPrefab;
    [SerializeField] public GameObject fallbackBallPrefab;

    private GameObject currentBall;

    void Start()
    {
        RuntimeManager.PlayOneShot("event:/Music/Music Excite");
        RuntimeManager.PlayOneShot("event:/Parameter Controllers/Battle Phase");
        // ensure your Inspector references are valid
        if (goldBallPrefab == null || diamondBallPrefab == null || defaultBallPrefab == null)
            Debug.LogError("[Sling] One or more ball‐prefab references missing.");

        // spawn the very first ball using the same priority you'd use in reload
        GameObject first = defaultBallPrefab;
        if (GameHandler.goldAmmo > 0)
        {
            first = goldBallPrefab;
            GameHandler.goldAmmo--;
        }
        else if (GameHandler.diamondAmmo > 0)
        {
            first = diamondBallPrefab;
            GameHandler.diamondAmmo--;
        }

        SpawnBall(first);
    }

    public void reload()
    {
        StartCoroutine(reloadNextBall());
    }

    private IEnumerator reloadNextBall()
    {
        yield return new WaitForSeconds(GameHandler.SLING_reload_time);

        // choose which ammo to consume
        GameObject prefabToSpawn = defaultBallPrefab;

        if (GameHandler.goldAmmo > 0)
        {
            prefabToSpawn = goldBallPrefab;
            GameHandler.goldAmmo--;
        }
        else if (GameHandler.diamondAmmo > 0)
        {
            prefabToSpawn = diamondBallPrefab;
            GameHandler.diamondAmmo--;
        }

        SpawnBall(prefabToSpawn);
    }

    private void SpawnBall()
    {
        SpawnBall(defaultBallPrefab);
    }

    private void SpawnBall(GameObject prefab)
    {
        if (currentBall != null && Vector3.Distance(gameObject.transform.position, currentBall.transform.position) < 0.25)
        {
            Debug.Log("TRIGGER");
            Destroy(currentBall);
        }

        if (prefab == null)
        {
            Debug.LogError("[Sling] Cannot spawn ball – prefab is null!");
            return;
        }

        currentBall = Instantiate(prefab, transform.position, Quaternion.identity);
        var bm = currentBall.GetComponent<BallMovement>();
        if (bm != null)
            bm.sling = this.gameObject;
        else
            Debug.LogWarning("[Sling] Spawned ball has no BallMovement component.");
    }

    public void ChangeBall(GameObject newBallPrefab)
    {
        nextBall = newBallPrefab;
        fallbackBallPrefab = newBallPrefab;

        if (currentBall != null)
            Destroy(currentBall);
        SpawnBall();
    }
}
