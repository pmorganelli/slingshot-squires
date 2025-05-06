using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ContinueBtn : MonoBehaviour
{
    public GameHandler gh;
    public GameObject noSeeds;
    public void Continue()
    {
        bool res = gh.startLevel();
        if (!res)
        {
            StartCoroutine(showNoSeeds());
        }
    }

    private IEnumerator showNoSeeds()
    {
        noSeeds.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        noSeeds.SetActive(false);
        SceneManager.LoadScene("ShopScene");
    }
}
