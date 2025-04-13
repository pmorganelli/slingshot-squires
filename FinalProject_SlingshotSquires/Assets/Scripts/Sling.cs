using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sling : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject nextBall;
    void Start()
    {
        Instantiate(nextBall, transform.position, Quaternion.identity);
    }

    public void reload()
    {
        StartCoroutine(reloadNextBall());
    }

    private IEnumerator reloadNextBall()
    {
        yield return new WaitForSeconds(GameHandler.SLING_reload_time);
        Instantiate(nextBall, transform.position, Quaternion.identity);
    }
}
