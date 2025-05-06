using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
public class PlayTitleMusic : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(startMusic());
    }

    private IEnumerator startMusic()
    {
        yield return new WaitForSeconds(1f);
        RuntimeManager.PlayOneShot("event:/Music/Play Music");
    }
}
