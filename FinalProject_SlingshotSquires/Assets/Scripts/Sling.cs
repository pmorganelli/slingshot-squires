using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sling : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject GameHandler;
    private GameHandler gh;
    public GameObject nextBall;
    void Start()
    {
        gh = GameHandler.GetComponent<GameHandler>();
        Instantiate(nextBall, transform.position, Quaternion.identity);
    }

    public void reload()
    {
        StartCoroutine(reloadNextBall());
    }

    private IEnumerator reloadNextBall()
    {
        yield return new WaitForSeconds(gh.SLING_reload_time);
        Instantiate(nextBall, transform.position, Quaternion.identity);
    }
}
