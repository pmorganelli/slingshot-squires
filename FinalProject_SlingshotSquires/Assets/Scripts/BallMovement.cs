using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    public string ballType = "default";
    public GameObject sling;
    public GameObject trajectoryDotPrefab;
    public int numberOfDots = 15;
    public float dotSpacing = 0.1f;

    public bool keyboardMode = false;

    private List<GameObject> dots;
    private Sling slingBehavior;
    private bool isPressed;
    private bool hasFired = false;
    private Rigidbody2D rb;
    private SpringJoint2D sj;
    private LineRenderer lr;
    private float releaseDelay;
    private float maxDragDistance = 1.75f;
    private Rigidbody2D slingRb;
    private AudioSource audioSource;

private void Awake()
{
    rb = GetComponent<Rigidbody2D>();
    sj = GetComponent<SpringJoint2D>();
    lr = GetComponent<LineRenderer>();
    audioSource = GetComponent<AudioSource>();

    GameObject foundSling = GameObject.FindGameObjectWithTag("Sling");
    if (foundSling != null)
    {
        sling = foundSling;
        slingBehavior = sling.GetComponent<Sling>();
        slingRb = sling.GetComponent<Rigidbody2D>();
    }
    else
    {
        Debug.LogError("[BallMovement] Could not find Sling in scene. Make sure it's tagged 'Sling'.");
    }

    if (sj != null && slingRb != null)
    {
        sj.connectedBody = slingRb;
        sj.enabled = true;
    }

    keyboardMode = GameHandler_PauseMenu.keyboardModeEnabled;
    hasFired = false;

    lr.enabled = false;

    releaseDelay = (sj != null && sj.frequency > 0) ? 1 / (sj.frequency * 4) : 0.05f;

    dots = new List<GameObject>();
    for (int i = 0; i < numberOfDots; i++)
    {
        GameObject dot = Instantiate(trajectoryDotPrefab);
        dot.transform.localScale = Vector3.one * 0.1f;
        dot.SetActive(false);
        dots.Add(dot);
    }
}


private bool lastKeyboardMode = false;

void Update()
    {
        keyboardMode = GameHandler_PauseMenu.keyboardModeEnabled;

        if (keyboardMode != lastKeyboardMode)
        {
            if (!keyboardMode)
            {
                HideTrajectory();
                rb.isKinematic = false;
            }

            lastKeyboardMode = keyboardMode;
        }

        if (keyboardMode && !hasFired)
        {
            rb.position = slingRb.position;
            rb.isKinematic = true;
            ShowKeyboardTrajectory();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                LaunchFromKeyboard();
            }
        }
        else if (isPressed)
        {
            DragBall();
            ShowTrajectory();
        }
    }



    private void DragBall()
    {
        if (slingRb == null) return;

        SetLineRendererPositions();
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float dist = Vector2.Distance(mousePos, slingRb.position);

        Vector2 newPos = (dist > maxDragDistance)
            ? slingRb.position + (mousePos - slingRb.position).normalized * maxDragDistance * GameHandler.SLING_force_multiplier
            : mousePos;

        rb.position = newPos;
    }

    private void SetLineRendererPositions()
    {
        if (slingRb == null) return;
        lr.SetPositions(new Vector3[] { rb.position, slingRb.position });
    }

    private void OnMouseDown()
    {
        if (keyboardMode || hasFired) return;
        isPressed = true;
        rb.isKinematic = true;
        lr.enabled = true;
    }

    private void OnMouseUp()
    {
        if (keyboardMode || hasFired) return;

        isPressed = false;
        rb.isKinematic = false;
        hasFired = true;

        // --- DEBUG: check slingBehavior reference & active state ---
        if (slingBehavior == null)
        {
            Debug.LogError("[BallMovement] slingBehavior is null!");
        }
        else
        {
            Debug.Log($"[BallMovement] Sling activeSelf={slingBehavior.gameObject.activeSelf} activeInHierarchy={slingBehavior.gameObject.activeInHierarchy}");
            
            // --- DEBUG: print parent chain ---
            Transform t = slingBehavior.transform;
            while (t != null)
            {
                Debug.Log($"[ParentCheck] {t.name} — activeSelf={t.gameObject.activeSelf}, activeInHierarchy={t.gameObject.activeInHierarchy}");
                t = t.parent;
            }
        }

        slingBehavior?.reload();
        StartCoroutine(Release());
        lr.enabled = false;
        HideTrajectory();
    }


    private IEnumerator Release()
    {
        audioSource.Play();
        yield return new WaitForSeconds(releaseDelay);
        sj.enabled = false;
    }



    private void ShowTrajectory()
    {
        if (slingRb == null) return;

        Vector2 launchPos = rb.position;
        Vector2 force = (slingRb.position - rb.position).normalized *
                        Mathf.Min(Vector2.Distance(rb.position, slingRb.position), maxDragDistance) *
                        (sj.frequency * 4f);

        float timeStep = dotSpacing;
        for (int i = 0; i < numberOfDots; i++)
        {
            float t = i * timeStep;
            Vector2 pos = launchPos + force * t;
            dots[i].transform.position = pos;
            dots[i].SetActive(true);
        }
    }

    private void ShowKeyboardTrajectory()
    {
        if (slingRb == null) return;

        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = slingRb.position - mouseWorldPos;
        Vector2 force = dir.normalized * Mathf.Min(dir.magnitude, maxDragDistance) * (sj.frequency * 4f);

        float timeStep = dotSpacing;
        Vector2 launchPos = slingRb.position;

        for (int i = 0; i < numberOfDots; i++)
        {
            float t = i * timeStep;
            Vector2 pos = launchPos + force * t;
            dots[i].transform.position = pos;
            dots[i].SetActive(true);
        }
    }

    private void LaunchFromKeyboard()
    {
        if (hasFired) return;
        hasFired = true;

        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = slingRb.position - mouseWorldPos;
        Vector2 force = dir.normalized * Mathf.Min(dir.magnitude, maxDragDistance) * (sj.frequency * 4f);

        rb.isKinematic = false;
        sj.enabled = false;

        rb.AddForce(force, ForceMode2D.Impulse);
        audioSource.Play();
        HideTrajectory();

        slingBehavior?.reload();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            destroyBall();
        }
    }

    public void destroyBall()
    {
        Destroy(gameObject);
    }

    private void HideTrajectory()
    {
        foreach (GameObject dot in dots)
        {
            dot.SetActive(false);
        }
    }
}
