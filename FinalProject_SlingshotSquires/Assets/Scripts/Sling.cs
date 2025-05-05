using UnityEngine;
using System.Collections;

public class Sling : MonoBehaviour
{
    [Header("Prefab Settings")]
    public GameObject nextBall;
    [SerializeField] public GameObject fallbackBallPrefab;

    private GameObject currentBall;

    void Start()
    {
        // Ensure nextBall is valid before first spawn
        if (nextBall == null && fallbackBallPrefab != null)
        {
            Debug.LogWarning("[Sling] nextBall was null at Start(), using fallback.");
            nextBall = fallbackBallPrefab;
        }

        if (nextBall != null)
        {
            SpawnBall();
        }
        else
        {
            Debug.LogError("[Sling] Cannot spawn ball — nextBall and fallback are both null.");
        }
    }

    public void reload()
    {
        Debug.Log($"[Sling.reload] activeSelf={gameObject.activeSelf} activeInHierarchy={gameObject.activeInHierarchy}");
        StartCoroutine(reloadNextBall());
    }

    private IEnumerator reloadNextBall()
    {
        yield return new WaitForSeconds(GameHandler.SLING_reload_time);

        if (this == null || gameObject == null)
            yield break;

        // Redundant check for safety after scene reload
        if (nextBall == null && fallbackBallPrefab != null)
        {
            Debug.LogWarning("[Sling] nextBall was null during reload — using fallback.");
            nextBall = fallbackBallPrefab;
        }

        if (nextBall != null)
        {
            Debug.Log("[Sling] Instantiating next ball");
            SpawnBall();
        }
        else
        {
            Debug.LogError("[Sling] Cannot spawn ball — both nextBall and fallbackBallPrefab are null.");
        }
    }

    private void SpawnBall() {
        if (currentBall != null)
            Destroy(currentBall);

        if (nextBall == null) {
            Debug.LogError("[Sling] Cannot spawn ball – prefab is null");
            return;
        }

        currentBall = Instantiate(nextBall, transform.position, Quaternion.identity);
        var bm = currentBall.GetComponent<BallMovement>();
        if (bm != null) bm.sling = this.gameObject;
    }

    public void ChangeBall(GameObject newBallPrefab) {
        nextBall = newBallPrefab;
        fallbackBallPrefab = newBallPrefab;

        if (currentBall != null)
            Destroy(currentBall);
        SpawnBall();
    }
}
