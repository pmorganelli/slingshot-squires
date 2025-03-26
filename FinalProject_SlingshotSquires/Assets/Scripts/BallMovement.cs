using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    private bool isPressed;
    private Rigidbody2D rb;
    private SpringJoint2D sj;

    private LineRenderer lr;
    private float releaseDelay;

    private float maxDragDistance = 3f;
    private Rigidbody2D slingRb;

    private TrailRenderer tr;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sj = GetComponent<SpringJoint2D>();
        lr = GetComponent<LineRenderer>();

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
            rb.position = slingRb.position + direction * maxDragDistance;
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
        StartCoroutine(Release());
        lr.enabled = false;
    }

    private IEnumerator Release()
    {
        yield return new WaitForSeconds(releaseDelay);
        sj.enabled = false;
        tr.enabled = true;
    }

}
