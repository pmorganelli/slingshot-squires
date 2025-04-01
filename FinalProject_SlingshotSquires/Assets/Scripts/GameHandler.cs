using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    public struct ballStat
    {
        public float damage;
        public float speed;

        public ballStat(float damage, float speed)
        {
            this.damage = damage;
            this.speed = speed;
        }
    };

    // Define a list to reference various ball stats based off the above struct.
    public Dictionary<string, ballStat> ballStats = new Dictionary<string, ballStat> {
        {"default", new ballStat(50f, 1.75f)}, {"fireball", new ballStat(75f, 2f)}
    };
    public float SLING_reload_time = 1f;
    public float SLING_force_multiplier = 1.25f;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void RestartGame()
    {
        GameHandler_PauseMenu.GameisPaused = false;
        SceneManager.LoadScene("peterSlingScene");
    }

    public void ReplayLastLevel()
    {
        GameHandler_PauseMenu.GameisPaused = false;
        SceneManager.LoadScene("peterSlingScene");
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

    public void loadCredits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void returnMain()
    {
        SceneManager.LoadScene("charlieScene");
    }

}
