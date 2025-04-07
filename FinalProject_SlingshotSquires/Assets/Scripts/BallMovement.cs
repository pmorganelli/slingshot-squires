using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    public string ballType = "default";
    public GameObject GameHandler;
    public GameObject sling;
    public GameObject trajectoryDotPrefab;
    public int numberOfDots = 15;
    public float dotSpacing = 0.1f;

    private List<GameObject> dots;
    private Sling slingBehavior;
    private GameHandler gh;
    private bool isPressed;
    private Rigidbody2D rb;
    private SpringJoint2D sj;
    private LineRenderer lr;
    private float releaseDelay;
    private float maxDragDistance = 1.75f;
    private Rigidbody2D slingRb;
    private TrailRenderer tr;
    private AudioSource audioSource;

    private void Awake()
    {
        slingBehavior = sling.GetComponent<Sling>();
        gh = GameHandler.GetComponent<GameHandler>();
        rb = GetComponent<Rigidbody2D>();
        sj = GetComponent<SpringJoint2D>();
        lr = GetComponent<LineRenderer>();
        audioSource = GetComponent<AudioSource>();
        tr = GetComponent<TrailRenderer>();
        slingRb = sj.connectedBody;

        lr.enabled = false;
        tr.enabled = false;

        releaseDelay = 1 / (sj.frequency * 4);

        // Initialize trajectory dots
        dots = new List<GameObject>();
        for (int i = 0; i < numberOfDots; i++)
        {
            GameObject dot = Instantiate(trajectoryDotPrefab);
            dot.transform.localScale = new Vector3(0.1f, 0.1f, 1f); // Small dots
            dot.SetActive(false);
            dots.Add(dot);
        }
    }

    void Update()
    {
        if (isPressed)
        {
            DragBall();
            ShowTrajectory();
        }
    }

    private void DragBall()
    {
        SetLineRendererPositions();
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float distance = Vector2.Distance(mousePosition, slingRb.position);

        if (distance > maxDragDistance)
        {
            Vector2 direction = (mousePosition - slingRb.position).normalized;
            rb.position = slingRb.position + direction * (maxDragDistance * gh.SLING_force_multiplier);
        }
        else
        {
            rb.position = mousePosition;
        }
    }

    private void SetLineRendererPositions()
    {
        Vector3[] positions = new Vector3[2];
        positions[0] = rb.position;
        positions[1] = slingRb.position;
        lr.SetPositions(positions);
    }

    private void OnMouseDown()
    {
        isPressed = true;
        rb.isKinematic = true;
        lr.enabled = true;
    }

    private void OnMouseUp()
    {
        isPressed = false;
        rb.isKinematic = false;
        slingBehavior.reload();
        StartCoroutine(Release());
        lr.enabled = false;
        HideTrajectory();
    }

    private IEnumerator Release()
    {
        audioSource.Play();
        yield return new WaitForSeconds(releaseDelay);
        sj.enabled = false;
        tr.enabled = true;
        yield return new WaitForSeconds(10f);
        destroyBall();
    }

    public void destroyBall()
    {
        Destroy(gameObject);
    }

    private void ShowTrajectory()
    {
        Vector2 launchPos = rb.position;
        Vector2 force = (slingRb.position - rb.position) * (sj.frequency * 4f); // Linear force direction
        float timeStep = dotSpacing;

        for (int i = 0; i < numberOfDots; i++)
        {
            float t = i * timeStep;
            Vector2 pos = launchPos + force * t; // No gravity, straight path
            dots[i].transform.position = pos;
            dots[i].SetActive(true);
        }
    }

    private void HideTrajectory()
    {
        foreach (var dot in dots)
        {
            dot.SetActive(false);
        }
    }
}
