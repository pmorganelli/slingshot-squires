using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayGame()
    {
        StartCoroutine(QuitAfter10());
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator QuitAfter10()
    {
        SceneManager.LoadScene("peterSlingScene");
        Debug.Log("Starting countdown");
        yield return new WaitForSeconds(10f);
        Debug.Log("Ending countdown");
        SceneManager.LoadScene("charlieScene");
    }
}
