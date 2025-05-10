using UnityEngine;
using System.Collections;
using FMODUnity;
public class Sling : MonoBehaviour
{
    [Header("Prefab Settings")]
    public static GameObject nextBall;
    public GameObject defaultBallPrefab;
    public GameObject goldBallPrefab;
    public GameObject diamondBallPrefab;

    public GameObject currentBall;

    void Start()
    {
        RuntimeManager.PlayOneShot("event:/Music/Music Excite");
        RuntimeManager.PlayOneShot("event:/Parameter Controllers/Battle Phase");
        // ensure your Inspector references are valid
        if (goldBallPrefab == null || diamondBallPrefab == null || defaultBallPrefab == null)
            Debug.LogError("[Sling] One or more ball‐prefab references missing.");

        // spawn the very first ball using the same priority you'd use in reload
        SpawnBall();
    }

    public void reload()
    {
        StartCoroutine(reloadNextBall());
    }

    private IEnumerator reloadNextBall()
    {
        yield return new WaitForSeconds(GameHandler.SLING_reload_time);

        // choose which ammo to consume
        SpawnBall();
    }

    private void SpawnBall()
    {
        if (currentBall != null && Vector3.Distance(gameObject.transform.position, currentBall.transform.position) < 0.25)
        {
            Debug.Log("TRIGGER");
            Destroy(currentBall);
        }

        if (nextBall == null)
        {
            Debug.Log("NULL NEXT--GOING DEFAULT");
            nextBall = defaultBallPrefab;
        }
        Debug.Log("SPAWNING BALL OF TYPE: " + nextBall.name);
        currentBall = Instantiate(nextBall, transform.position, Quaternion.identity);
        var bm = currentBall.GetComponent<BallMovement>();
        if (bm != null)
            bm.sling = this.gameObject;
        else
            Debug.LogWarning("[Sling] Spawned ball has no BallMovement component.");
    }

    public static void ChangeBall(GameObject newBallPrefab)
    {
        nextBall = newBallPrefab;
    }
}