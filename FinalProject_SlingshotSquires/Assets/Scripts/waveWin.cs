using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class waveWin : MonoBehaviour
{
    // Start is called before the first frame update
    public void nextLevel()
    {
        SceneManager.LoadScene("TitleScreen");
    }
}
