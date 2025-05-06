using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
public class LoseScript : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        RuntimeManager.PlayOneShot("event:/SFX/Wave Lose");
    }
}
