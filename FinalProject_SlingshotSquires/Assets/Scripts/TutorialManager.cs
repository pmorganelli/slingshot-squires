using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
public class TutorialManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (GameHandler.levelCount == 0)
        {
            gameObject.SetActive(true);
        }

        else
        {
            gameObject.SetActive(false);
        }
    }
    public GameObject nextButton;
    public GameObject waveButton;
    public GameObject textA;
    public GameObject textB;
    public GameObject textC;
    public GameObject textD;
    public GameObject tutBG;
    public int textNum = 0;


    public void nextWasClicked()
    {
        RuntimeManager.PlayOneShot("event:/SFX/UI Interact");
        if (textNum == 0)
        {
            textA.gameObject.SetActive(false);
            textB.gameObject.SetActive(true);
            textNum++;
        }
        else if (textNum == 1)
        {
            textB.gameObject.SetActive(false);
            textC.gameObject.SetActive(true);
            textNum++;
        }
        else
        {
            nextButton.SetActive(false);
            textC.gameObject.SetActive(false);
            textD.gameObject.SetActive(true);
            waveButton.gameObject.SetActive(true);
        }
    }

    public void waveWasClicked()
    {
        RuntimeManager.PlayOneShot("event:/SFX/UI Interact");
        textD.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);
        waveButton.gameObject.SetActive(false);
        tutBG.gameObject.SetActive(false);
        GameHandler.waveStarted = true;
    }
}
