using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorManager : MonoBehaviour
{
    public GameHandler gh;
    public GameObject insuffSeedsTxt;
    private void OnCollisionEnter2D(Collision2D other)
    {
        bool res = gh.startLevel();
        if (!res)
        {
            Debug.Log("FLASHING");
            StartCoroutine(flashInsuffSeedsTxt());
        }
    }

    private IEnumerator flashInsuffSeedsTxt()
    {
        Debug.Log("ON");
        insuffSeedsTxt.SetActive(true);
        yield return new WaitForSeconds(2f);
        Debug.Log("OFF");
        insuffSeedsTxt.SetActive(false);
    }
}
