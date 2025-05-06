using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMODUnity;
public class DoorManager : MonoBehaviour
{
    public GameHandler gh;
    public GameObject insuffSeedsTxt;
    private void OnCollisionEnter2D(Collision2D other)
    {
        bool res = gh.startLevel();
        if (!res)
        {
            StartCoroutine(flashInsuffSeedsTxt());
        }
    }

    private IEnumerator flashInsuffSeedsTxt()
    {
        RuntimeManager.PlayOneShot("event:/SFX/UI Fail");
        insuffSeedsTxt.SetActive(true);
        yield return new WaitForSeconds(2f);
        insuffSeedsTxt.SetActive(false);
    }
}
