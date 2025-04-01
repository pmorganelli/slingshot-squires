using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    public string ballType = "default";
    public GameObject GameHandler;
    public GameObject sling;
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
    }

    void Update()
    {
        if (isPressed)
        {
            DragBall();
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
    }

    private IEnumerator Release()
    {
        audioSource.Play();
        yield return new WaitForSeconds(releaseDelay);
        sj.enabled = false;
        tr.enabled = false;
        yield return new WaitForSeconds(10f);
        destroyBall();
    }



    public void destroyBall()
    {
        Destroy(gameObject);
    }
}
