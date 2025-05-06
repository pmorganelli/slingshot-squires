using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
public class PlayTitleMusic : MonoBehaviour
{
    private void Awake()
    {
        RuntimeManager.PlayOneShot("event:/Music/Play Music");
    }
}
